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
        private readonly IMapper _mapper;

        public FileManagementService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IMapper mapper
            )
        {
            _fileDatabaseService = fileDatabaseService;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<FileDtoCreateResponse> AddFileToDatabaseAndStorageAsync(IFormFile fileData, Guid folderId)
        {
            Guid fileId = Guid.Empty;

            FileDtoCreateResponse fileDtoCreateResponse;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    fileId = await _fileStorageService.UploadFileAsync(fileData, folderId);

                    FileDtoCreateRequest fileDtoCreateRequest = _mapper.Map<FileDtoCreateRequest>(fileData);

                    fileDtoCreateRequest.StorageFileId = fileId;

                    fileDtoCreateRequest.FolderId = folderId;

                    fileDtoCreateResponse = await _fileDatabaseService.CreateFileAsync(fileDtoCreateRequest);

                    transaction.Complete();

                    return fileDtoCreateResponse;
                }
                catch (Exception ex)
                {
                    if (fileId != Guid.Empty)
                    {
                        await _fileStorageService.DeleteFileAsync(fileId, folderId);
                    }

                    throw new ApplicationException("An error occurred while creating the file.", ex);
                }
            }
        }
    }
}
