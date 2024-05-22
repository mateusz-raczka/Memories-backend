using System.Transactions;
using MemoriesBackend.Application.Interfaces.Services;
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

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService
        )
        {
            _fileDatabaseService = fileDatabaseService;
            _fileStorageService = fileStorageService;
            _folderDatabaseService = folderDatabaseService;
        }

        public async Task<File> AddFileToDatabaseAndStorageAsync(IFormFile fileData, Guid folderId)
        {
            var fileId = Guid.Empty;

            var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
            if (folder == null) throw new ArgumentException("Folder with the given ID does not exist.", nameof(folderId));

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    fileId = await _fileStorageService.UploadFileAsync(fileData, folderId);

                    var file = new File()
                    {
                        Id = fileId,
                        FolderId = folderId,
                        FileDetails = new FileDetails()
                        {
                            Name = fileData.Name,
                            Size = fileData.Length
                        }
                    };

                    var createdFile = await _fileDatabaseService.CreateFileAsync(file);

                    transactionScope.Complete();

                    return createdFile;
                }
                catch (Exception ex)
                {
                    if (fileId != Guid.Empty) await _fileStorageService.DeleteFileAsync(fileId);

                    throw new ApplicationException("An error occurred while creating the file.", ex);
                }
            }
        }
    }
}