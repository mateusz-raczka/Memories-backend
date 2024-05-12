using AutoMapper;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Services.Interfaces;
using System.Transactions;

namespace Memories_backend.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly IMapper _mapper;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            IMapper mapper
            )
        {
            _fileDatabaseService = fileDatabaseService;
            _fileStorageService = fileStorageService;
            _folderDatabaseService = folderDatabaseService;
            _mapper = mapper;
        }

        public async Task<FileDtoCreateResponse> AddFileToDatabaseAndStorageAsync(IFormFile fileData, Guid folderId)
        {
            Guid fileId = Guid.Empty;

            var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
            if (folder == null)
            {
                throw new ArgumentException("Folder with the given ID does not exist.", nameof(folderId));
            }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    fileId = await _fileStorageService.UploadFileAsync(fileData, folderId);

                    var fileDtoCreateRequest = _mapper.Map<FileDtoCreateRequest>(fileData);

                    fileDtoCreateRequest.Id = fileId;
                    fileDtoCreateRequest.FolderId = folderId;

                    var fileDtoCreateResponse = await _fileDatabaseService.CreateFileAsync(fileDtoCreateRequest);

                    transactionScope.Complete();

                    return fileDtoCreateResponse;
                }
                catch (Exception ex)
                {
                    if (fileId != Guid.Empty)
                    {
                        await _fileStorageService.DeleteFileAsync(fileId);
                    }

                    throw new ApplicationException("An error occurred while creating the file.", ex);
                }
            }
        }

    }
}
