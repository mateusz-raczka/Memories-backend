using MemoriesBackend.Application.Interfaces;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Domain.Models;
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
        private readonly IFileUploadProgressDatabaseService _fileUploadProgressDatabaseService;
        private readonly IFileChunkDatabaseService _fileChunkDatabaseService;
        private readonly ITransactionHandler _transactionHandler;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            IPathService pathService,
            IFileChunkDatabaseService fileChunkDatabaseService,
            IFileUploadProgressDatabaseService fileUploadProgressDatabaseService,
            ITransactionHandler transactionHandler
            )
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _pathService = pathService;
            _fileChunkDatabaseService = fileChunkDatabaseService;
            _fileUploadProgressDatabaseService = fileUploadProgressDatabaseService;
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
                FileUploadProgressId = fileId,
                Size = stream.Length,
                ChunkIndex = chunkIndex
            };

            await _fileChunkDatabaseService.CreateFileChunkAsync(fileChunk);
            await _fileChunkDatabaseService.SaveAsync();

            var isUploaded = currentUploadProgress.FileChunks.Count + 1 >= totalChunks;

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

                _fileUploadProgressDatabaseService.DeleteFileUploadProgressAsync(currentUploadProgress);
            }
            else
            {
                currentUploadProgress.LastModifiedDate = DateTime.UtcNow;
                currentUploadProgress.Size += stream.Length;

                _fileUploadProgressDatabaseService.UpdateFileUploadProgressAsync(currentUploadProgress);
            }

            await _fileUploadProgressDatabaseService.SaveAsync();

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

            var pastedFilesToStorage = await CopyAndPasteFilesInStorageAsync(filesToCopy, targetFolderId);

            var pastedFiles = new List<File>();

            foreach (var pastedFileToStorage in pastedFilesToStorage)
            {
                var pastedFile = await CopyFileAsync(pastedFileToStorage.CopiedFile, targetFolderId, pastedFileToStorage.PastedFileId);
                pastedFiles.Add(pastedFile);
            }

            await _fileDatabaseService.SaveAsync();

            return pastedFiles;
        }

        public async Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId)
        {
            var filesToCopy = await _fileDatabaseService.GetAllFilesAsync(
                f => filesIdsToCopy.Contains(f.Id)
                );

            var pastedFiles = await CopyAndPasteFilesAsync(filesToCopy, targetFolderId);

            return pastedFiles;
        }

        public async Task<IEnumerable<File>> MoveFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var filesToCopy = await _fileDatabaseService.GetAllFilesAsync(
                    f => filesIdsToCopy.Contains(f.Id)
                );

            var pastedFiles = new List<File>();

            foreach (var file in filesToCopy)
            {
                var pastedFile = await MoveFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            await MoveFilesToStorageAsync(filesToCopy, targetFolderId);

            await _fileDatabaseService.SaveAsync();

            return pastedFiles;
        }

        public async Task<IEnumerable<File>> MoveFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach( var file in filesToCopy)
            {
                var pastedFile = await MoveFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            await MoveFilesToStorageAsync(filesToCopy, targetFolderId);

            await _fileDatabaseService.SaveAsync();

            return pastedFiles;
        }

        private async Task<FileUploadProgress> GetOrInitializeUploadProgressAsync(int chunkIndex, int totalChunks, Guid folderId, Guid fileId, string fileName)
        {
            var currentUploadProgress = await _fileUploadProgressDatabaseService.GetFileUploadProgressByIdWithRelationsAsync(fileId);

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

                await _fileUploadProgressDatabaseService.CreateFileUploadProgressAsync(currentUploadProgress);
            }

            return currentUploadProgress;
        }

        private async Task<File> MoveFileAsync(File file, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var folderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);
            var fileAbsolutePath = await _pathService.GetFileAbsolutePathAsync(file.Id);
            await _fileStorageService.MoveFileAsync(fileAbsolutePath, folderAbsolutePath);

            file.FolderId = targetFolderId;

            _fileDatabaseService.UpdateFile(file);

            return file;
        }

        private async Task MoveFilesToStorageAsync(IEnumerable<File> filesToCopy, Guid targetFolderId)
        {
            var folderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);

            var pasteFilesToStorageTasks = filesToCopy.Select(file => MoveFileInStorageAsync(file, folderAbsolutePath));

            await Task.WhenAll(pasteFilesToStorageTasks);
        }

        private async Task MoveFileInStorageAsync(File file, string folderAbsolutePath)
        {
            var fileAbsolutePath = Path.Combine(folderAbsolutePath, file.Id + file.FileDetails.Extension);

            await _fileStorageService.MoveFileAsync(fileAbsolutePath, folderAbsolutePath);
        }

        private async Task<PastedFileToStorage> CopyAndPasteFileInStorageAsync(File fileToCopy, string folderAbsolutePath)
        {
            var fileAbsolutePath = Path.Combine(folderAbsolutePath, fileToCopy.Id + fileToCopy.FileDetails.Extension);

            var pastedFile = new PastedFileToStorage
            {
                CopiedFile = fileToCopy,
                PastedFileId = await _fileStorageService.CopyAndPasteFileAsync(fileAbsolutePath, folderAbsolutePath)
            };

            return pastedFile;
        }

        private async Task<IEnumerable<PastedFileToStorage>> CopyAndPasteFilesInStorageAsync(IEnumerable<File> filesToCopy, Guid targetFolderId)
        {
            var folderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);

            var pasteFilesToStorageTasks = filesToCopy.Select(file => CopyAndPasteFileInStorageAsync(file, folderAbsolutePath));

            return await Task.WhenAll(pasteFilesToStorageTasks);
        }

        private async Task<File> CopyFileAsync(File fileToCopy, Guid targetFolderId, Guid fileStorageId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId, false);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var fileCopy = new File
            {
                Id = fileStorageId,
                FolderId = targetFolderId,
                FileDetails = new FileDetails
                {
                    Id = fileStorageId,
                    Name = fileToCopy.FileDetails.Name,
                    Size = fileToCopy.FileDetails.Size,
                    Extension = fileToCopy.FileDetails.Extension,
                }
            };

            var pastedFile = await _fileDatabaseService.CreateFileAsync(fileCopy);

            return pastedFile;
        }
    }
}
