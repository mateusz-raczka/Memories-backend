using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
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
        private readonly ITransactionHandler _transactionHandler;
        private readonly IPathService _pathService;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            ITransactionHandler transactionHandler,
            IPathService pathService
            )
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _transactionHandler = transactionHandler;
            _pathService = pathService;
        }

        public async Task<File> AddFileAsync(IFormFile fileData, Guid folderId)
        {
            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync( folderId );

            return await _transactionHandler.ExecuteAsync(async () =>
            {
                var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
                if (folder == null) throw new ArgumentException("Folder with the given ID does not exist.", nameof(folderId));

                var fileId = await _fileStorageService.UploadFileAsync(fileData, absoluteFolderPath);

                var file = new File
                {
                    Id = fileId,
                    FolderId = folderId,
                    FileDetails = new FileDetails
                    {
                        Name = fileData.FileName,
                        Size = fileData.Length,
                        Extension = Path.GetExtension(fileData.FileName)
                    }
                };

                var createdFile = await _fileDatabaseService.CreateFileAsync(file);
                return createdFile;
            });
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
    }
}
