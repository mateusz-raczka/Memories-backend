using Microsoft.AspNetCore.Mvc;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.File.Request;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Memories_backend.Services.Interfaces;
using Memories_backend.Services;

namespace Memories_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FileController : ControllerBase
    {
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFileManagementService _fileManagementService;

        public FileController(
            IFileDatabaseService fileDatabaseService,
            IFileManagementService fileManagementService
            )
        {
            _fileDatabaseService = fileDatabaseService;
            _fileManagementService = fileManagementService;
        }

        [HttpGet]
        public async Task<IEnumerable<FileDtoFetchResponse>> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string? filterName = null
            )
        {
            Expression<Func<Models.Domain.File, bool>> filter = null;

            if (filterName != null)
            {
                filter = entity => entity.FileDetails.Name.Contains(filterName);
            }

            Func<IQueryable<Models.Domain.File>, IOrderedQueryable<Models.Domain.File>> orderBy = query => query.OrderBy(entity => entity.FileDetails.Name);

            IEnumerable<FileDtoFetchResponse> response = await _fileDatabaseService.GetAllFiles(
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
        public async Task<FileDtoCreateResponse> Add(IFormFile fileData, Guid folderId)
        {
            FileDtoCreateResponse response = await _fileManagementService.AddFileToDatabaseAndStorageAsync(fileData, folderId);

            return response;
        }
        
        [HttpPut("{id:Guid}")]
        public async Task Update(Guid id, [FromBody] FileDtoUpdateRequest updatedFileDto)
        {
            await _fileDatabaseService.UpdateFileAsync(id, updatedFileDto);
        }
        
        [HttpDelete("{id:Guid}")]
        public async Task Delete(Guid id)
        {
            await _fileDatabaseService.DeleteFileAsync(id);
        }
    }
}
