using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models.FileStorage;
using MemoriesBackend.Domain.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    internal sealed class FileManagementService : IFileManagementService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly IPathService _pathService;
        private readonly IFolderStorageService _folderStorageService;
        private readonly IGenericRepository<FileUploadProgress> _fileUploadProgressRepository;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            IPathService pathService,
            IFolderStorageService folderStorageService,
            IGenericRepository<FileUploadProgress> fileUploadProgressRepository
            )
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _pathService = pathService;
            _folderStorageService = folderStorageService;
            _fileUploadProgressRepository = fileUploadProgressRepository;
        }

        public async Task<File> AddFileAsync(IFormFile fileData, Guid folderId)
        {
            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync( folderId );
            FileUploadedResult uploadedFile = null;

                var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
                if (folder == null) throw new ApplicationException($"Folder with the given ID {folderId} - does not exist.");

                uploadedFile = await _fileStorageService.UploadFileAsync(fileData, absoluteFolderPath);

                if(uploadedFile == null)
                {
                    throw new ApplicationException("Cannot get file id from uploaded file");
                }

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

                return createdFile;
        }

        public async Task<FileChunkUploadedResult> AddFileChunkAsync(Stream stream, string fileName, int chunkIndex, int totalChunks, Guid folderId, Guid fileId)
        {
            var currentUploadProgress = await _fileUploadProgressRepository.GetById(fileId);

            if (currentUploadProgress == null)
            {
                if(chunkIndex != 0)
                {
                    throw new ApplicationException("Unable to find progress of the file");
                }

                var relativePath = await _pathService.GetFolderRelativePathAsync(folderId);

                currentUploadProgress = new FileUploadProgress
                {
                    Id = fileId,
                    ChunkIndex = 0,
                    TotalChunks = totalChunks,
                    RelativePath = relativePath,
                    LastModifiedDate = DateTime.UtcNow
                };

                await _fileUploadProgressRepository.Create(currentUploadProgress);
            }

            if (chunkIndex < currentUploadProgress.ChunkIndex)
            {
                throw new ApplicationException("Invalid chunk index.");
            }

            string absoluteFolderPath = _pathService.GetAbsolutePath(currentUploadProgress.RelativePath);

            var fileChunkMetaData = new FileChunkMetaData
            {
                Id = fileId,
                ChunkIndex = chunkIndex,
                TotalChunks = totalChunks,
                FileName = fileName
            };

            await _fileStorageService.UploadFileChunkAsync(stream, fileChunkMetaData, absoluteFolderPath);

            currentUploadProgress.ChunkIndex = chunkIndex + 1;
            currentUploadProgress.LastModifiedDate = DateTime.UtcNow;
            currentUploadProgress.Size += stream.Length;

            var isUploaded = currentUploadProgress.ChunkIndex >= totalChunks;

            if (isUploaded)
            {
                var file = new File
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
                await _fileUploadProgressRepository.Delete(fileId);
            }

            _fileUploadProgressRepository.Update(currentUploadProgress);
            await _fileUploadProgressRepository.Save();

            return new FileChunkUploadedResult
            {
                Id = currentUploadProgress.Id,
                isUploaded = isUploaded
            };
        }


        public async Task<FileStreamResult> StreamFileAsync(Guid fileId)
        {
            var file = _fileDatabaseService.GetFileByIdAsync(fileId);
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);
            return _fileStorageService.StreamFile(absoluteFilePath);
        }

        public async Task DeleteFolderAsync(Guid folderId)
        {
            var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
            if(folder == null)
            {
                throw new ApplicationException($"Cannot delete folder with Id {folderId} - it was not found");
            }

            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync(folderId);

            await _folderDatabaseService.DeleteFolderAsync(folderId);
            await _folderStorageService.DeleteFolderAsync(absoluteFolderPath);
        }

        public async Task DeleteFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);

                await _fileDatabaseService.DeleteFileAsync(fileId);
                _fileStorageService.DeleteFile(absoluteFilePath);
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

                var folderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);
                var fileAbsolutePath = await _pathService.GetFileAbsolutePathAsync(file.Id);
                var pastedFileId = await _fileStorageService.CopyAndPasteFileAsync(fileAbsolutePath, folderAbsolutePath);

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

                return await _fileDatabaseService.CreateFileAsync(fileCopy);
        }
    }
}
