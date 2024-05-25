using MemoriesBackend.Application.Interfaces;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Application.Interfaces.Transactions;
using MemoriesBackend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly ITransactionHandler _transactionHandler;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            ITransactionHandler transactionHandler)
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _transactionHandler = transactionHandler;
        }

        public async Task<File> AddFileToDatabaseAndStorageAsync(IFormFile fileData, Guid folderId)
        {
            return await _transactionHandler.ExecuteAsync(async () =>
            {
                var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
                if (folder == null) throw new ArgumentException("Folder with the given ID does not exist.", nameof(folderId));

                var fileId = await _fileStorageService.UploadFileAsync(fileData, folderId);

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
    }
}
