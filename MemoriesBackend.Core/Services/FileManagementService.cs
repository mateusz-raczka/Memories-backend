using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly IPathService _pathService;
        private readonly IGenericRepository<FileUploadProgress> _fileUploadProgressRepository;
        private readonly IGenericRepository<FileChunk> _fileChunkRepository;
        private readonly ITransactionHandler _transactionHandler;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            IPathService pathService,
            IGenericRepository<FileUploadProgress> fileUploadProgressRepository,
            IGenericRepository<FileChunk> fileChunkRepository,
            ITransactionHandler transactionHandler
            )
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _pathService = pathService;
            _fileUploadProgressRepository = fileUploadProgressRepository;
            _fileChunkRepository = fileChunkRepository;
            _transactionHandler = transactionHandler;
        }

        public async Task<File> AddFileAsync(IFormFile fileData, Guid folderId)
        {
            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync(folderId);

            var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
            if (folder == null)
                throw new ApplicationException($"Folder with the given ID {folderId} - does not exist.");

            var uploadedFile = await _fileStorageService.UploadFileAsync(fileData, absoluteFolderPath);

            if (uploadedFile == null)
                throw new ApplicationException("Cannot get file id from uploaded file");

            var file = new File
            {
                Id = uploadedFile.Id,
                FolderId = folderId,
                FileDetails = new FileDetails
                {
                    Id = uploadedFile.Id,
                    Name = uploadedFile.Name,
                    Size = uploadedFile.Size,
                    Extension = uploadedFile.Extension
                }
            };

            var createdFile = await _fileDatabaseService.CreateFileAsync(file);
            await _fileDatabaseService.SaveAsync();

            return createdFile;
        }

        public async Task<File?> AddFileUsingChunksAsync(Stream stream, string fileName, int chunkIndex, int totalChunks, Guid folderId, Guid fileId)
        {
            File file = null;

            var currentUploadProgress = await GetOrInitializeUploadProgressAsync(chunkIndex, totalChunks, folderId, fileId, fileName);

            if (chunkIndex != currentUploadProgress.FileChunks.Count)
                throw new ApplicationException("Invalid chunk index.");
            if (chunkIndex > currentUploadProgress.FileChunks.Count)
                throw new ApplicationException("File is already uploaded");

            var absoluteFolderPath = _pathService.GetAbsolutePath(currentUploadProgress.RelativePath);

            var fileChunkId = await _fileStorageService.UploadFileChunkAsync(stream, absoluteFolderPath, currentUploadProgress.Id);

            var fileChunk = new FileChunk
            {
                Id = fileChunkId,
                FileId = fileId,
                Size = stream.Length,
                ChunkIndex = chunkIndex
            };

            var newFileChunk = await _fileChunkRepository.Create(fileChunk);
            currentUploadProgress.FileChunks.Add(newFileChunk);

            currentUploadProgress.LastModifiedDate = DateTime.UtcNow;
            currentUploadProgress.Size += stream.Length;

            var isUploaded = currentUploadProgress.FileChunks.Count >= totalChunks;


            if (isUploaded)
            {
                await _fileStorageService.MergeAndDeleteFileChunksAsync(currentUploadProgress, absoluteFolderPath);

                file = new File
                {
                    Id = fileId,
                    FolderId = folderId,
                    FileDetails = new FileDetails
                    {
                        Id = fileId,
                        Name = fileName,
                        Size = currentUploadProgress.Size,
                        Extension = Path.GetExtension(fileName)
                    }
                };

                await _fileDatabaseService.CreateFileAsync(file);
                await _fileDatabaseService.SaveAsync();
                await _fileUploadProgressRepository.Delete(fileId);
                foreach(var chunk in currentUploadProgress.FileChunks)
                {
                    await _fileChunkRepository.Delete(chunk.Id);
                }
            }
            else
            {
                _fileUploadProgressRepository.Update(currentUploadProgress);
            }

            await _fileUploadProgressRepository.Save();
            await _fileChunkRepository.Save();

            return file;
        }


        public async Task<FileStreamResult> StreamFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);
            return await _fileStorageService.StreamFileAsync(absoluteFilePath);
        }

        public async Task DeleteFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);

            await _fileDatabaseService.DeleteFileAsync(fileId);
            _fileStorageService.DeleteFile(absoluteFilePath);
            await _fileDatabaseService.SaveAsync();
        }

        public async Task<FileContentResult> DownloadFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);

            return await _fileStorageService.DownloadFileAsync(absoluteFilePath);
        }

        public async Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach (var file in filesToCopy)
            {
                var pastedFile = await CopyAndPasteFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            return pastedFiles;
        }

        public async Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach (var fileId in filesIdsToCopy)
            {
                var file = await _fileDatabaseService.GetFileByIdAsync(fileId);
                var pastedFile = await CopyAndPasteFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            return pastedFiles;
        }

        public async Task<File> CopyAndPasteFileAsync(File file, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var targetFolderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);
            var fileAbsolutePath = await _pathService.GetFileAbsolutePathAsync(file.Id);
            var pastedFileId = await _fileStorageService.CopyAndPasteFileAsync(fileAbsolutePath, targetFolderAbsolutePath);

            var fileCopy = new File
            {
                Id = pastedFileId,
                FolderId = targetFolderId,
                FileDetails = new FileDetails
                {
                    Id = pastedFileId,
                    Name = file.FileDetails.Name,
                    Size = file.FileDetails.Size,
                    Extension = file.FileDetails.Extension,
                }
            };

            var pastedFile = await _fileDatabaseService.CreateFileAsync(fileCopy);
            await _fileDatabaseService.SaveAsync();

            return pastedFile;
        }

        public async Task<IEnumerable<File>> CutAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach (var fileId in filesIdsToCopy)
            {
                var file = await _fileDatabaseService.GetFileByIdAsync(fileId);
                var pastedFile = await CutAndPasteFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            return pastedFiles;
        }

        public async Task<IEnumerable<File>> CutAndPasteFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach (var file in filesToCopy)
            {
                var pastedFile = await CutAndPasteFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            return pastedFiles;
        }

        public async Task<File> CutAndPasteFileAsync(File file, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var folderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);
            var fileAbsolutePath = await _pathService.GetFileAbsolutePathAsync(file.Id);
            await _fileStorageService.CutAndPasteFileAsync(fileAbsolutePath,folderAbsolutePath);

            file.FolderId = targetFolderId;

            await _folderDatabaseService.SaveAsync();

            return file;
        }

        private async Task<FileUploadProgress> GetOrInitializeUploadProgressAsync(int chunkIndex, int totalChunks, Guid folderId, Guid fileId, string fileName)
        {
            var currentUploadProgress = await _fileUploadProgressRepository.GetById(fileId);

            if (currentUploadProgress == null)
            {
                if (chunkIndex != 0)
                {
                    throw new ApplicationException($"Unable to find upload progress for file with Id {fileId}");
                }

                var relativeFolderPath = await _pathService.GetFolderRelativePathAsync(folderId);
                currentUploadProgress = new FileUploadProgress
                {
                    Id = fileId,
                    TotalChunks = totalChunks,
                    RelativePath = relativeFolderPath,
                    Extension = Path.GetExtension(fileName),
                    Name = Path.GetFileName(fileName),
                    LastModifiedDate = DateTime.UtcNow,
                    FileChunks = new List<FileChunk>()
                };

                await _fileUploadProgressRepository.Create(currentUploadProgress);
                await _fileUploadProgressRepository.Save();
            }

            return currentUploadProgress;
        }
    }
}
