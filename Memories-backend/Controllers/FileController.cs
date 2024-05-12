using Microsoft.AspNetCore.Mvc;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.File.Request;
using System.Linq.Expressions;
using Memories_backend.Services.Interfaces;

namespace Memories_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFileManagementService _fileManagementService;
        private readonly IFileStorageService _fileStorageService;

        public FileController(
            IFileDatabaseService fileDatabaseService,
            IFileManagementService fileManagementService,
            IFileStorageService fileStorageService
            )
        {
            _fileDatabaseService = fileDatabaseService;
            _fileManagementService = fileManagementService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<FileDtoFetchResponse>> GetAll(
            int? pageNumber,
            int? pageSize,
            string? filterName = null
            )
        {
            Expression<Func<Models.Domain.File, bool>> filter = null;

            if (!string.IsNullOrEmpty(filterName))
            {
                filter = entity => entity.FileDetails.Name.Contains(filterName);
            }

            // Hardcoded orderby name
            Func<IQueryable<Models.Domain.File>, IOrderedQueryable<Models.Domain.File>> orderBy = query => query.OrderBy(entity => entity.FileDetails.Name);

            IEnumerable<FileDtoFetchResponse> response = await _fileDatabaseService.GetAllFilesAsync(
                pageNumber, 
                pageSize, 
                filter, 
                orderBy
                );

            return response;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<FileDtoFetchResponse> GetById(Guid id)
        {
            FileDtoFetchResponse response = await _fileDatabaseService.GetFileByIdAsync(id);

            return response;
        }
        
        [HttpPost("{folderId:Guid}")]
        public async Task<FileDtoCreateResponse> Add([FromForm] IFormFile fileData, Guid folderId)
        {
            FileDtoCreateResponse response = await _fileManagementService.AddFileToDatabaseAndStorageAsync(fileData, folderId);

            return response;
        }
        
        [HttpPut("{id:Guid}")]
        public async Task Update(Guid id, [FromBody] FileDtoUpdateRequest updateModel)
        {
            await _fileDatabaseService.UpdateFileAsync(id, updateModel);
        }
        
        [HttpDelete("{id:Guid}")]
        public async Task Delete(Guid id)
        {
            await _fileDatabaseService.DeleteFileAsync(id);
        }

        [HttpGet("Download/{id:Guid}")]
        public async Task<FileContentResult> Download(Guid id)
        {
            return await _fileStorageService.DownloadFileAsync(id);
        }
    }
}
