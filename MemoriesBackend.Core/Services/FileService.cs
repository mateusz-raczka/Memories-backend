using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Domain.Models.FileManagement;
using MemoriesBackend.Domain.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    internal sealed class FileService : IFileService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly ITransactionHandler _transactionHandler;
        private readonly IPathService _pathService;
        private readonly IGenericRepository<Folder> _folderRepository;
        private readonly IFolderStorageService _folderStorageService;

        public FileService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            ITransactionHandler transactionHandler,
            IPathService pathService,
            IGenericRepository<Folder> folderRepository,
            IFolderStorageService folderStorageService
            )
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _transactionHandler = transactionHandler;
            _pathService = pathService;
            _folderRepository = folderRepository;
            _folderStorageService = folderStorageService;
        }

        public async Task<File> AddFileAsync(IFormFile fileData, Guid folderId)
        {
            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync( folderId );
            UploadFileResult uploadedFile = null;

            return await _transactionHandler.ExecuteAsync(async () =>
            {
                var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
                if (folder == null) throw new ApplicationException($"Folder with the given ID {folderId} - does not exist.");

                uploadedFile = await _fileStorageService.UploadFileAsync(fileData, absoluteFolderPath);

                if(uploadedFile == null)
                {
                    throw new ApplicationException("Failed to upload file to file storage");
                }

                var file = new File
                {
                    Id = uploadedFile.Id,
                    FolderId = folderId,
                    FileDetails = new FileDetails
                    {
                        Id = uploadedFile.Id,
                        Name = fileData.FileName,
                        Size = fileData.Length,
                        Extension = Path.GetExtension(fileData.FileName)
                    }
                };

                var createdFile = await _fileDatabaseService.CreateFileAsync(file);

                return createdFile;

            }, async () => await UploadFileRollbackAsync(uploadedFile, absoluteFolderPath));
        }

        public async Task<FileStreamResult> StreamFileAsync(Guid fileId)
        {
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

            await _transactionHandler.ExecuteAsync(async () =>
            {
                await _fileDatabaseService.DeleteFileAsync(fileId);
                await _fileStorageService.DeleteFileAsync(absoluteFilePath);
            });
        }

        public async Task<FileContentResult> DownloadFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);

            return await _fileStorageService.DownloadFileAsync(absoluteFilePath);
        }
        
        private async Task UploadFileRollbackAsync(UploadFileResult uploadFileResult, string absoluteFolderPath)
        {
            try
            {
                var absoluteFilePath = Path.Combine(absoluteFolderPath, uploadFileResult.Id.ToString());
                if (System.IO.File.Exists(absoluteFilePath))
                {
                    await _fileStorageService.DeleteFileAsync(absoluteFolderPath);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to rollback file", ex);
            }
        }
    }
}
