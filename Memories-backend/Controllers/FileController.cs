using Microsoft.AspNetCore.Mvc;
using File = Memories_backend.Models.Domain.File;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.File.Request;
using AutoMapper;
using System.Linq.Expressions;
using Memories_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Memories_backend.Models.DTO.Identity.Roles;

namespace Memories_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<FileDtoFetchResponse>> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string? filterName = null
            )
        {
            Expression<Func<File, bool>> filter = null;

            if (filterName != null)
            {
                filter = entity => entity.FileDetails.Name.Contains(filterName);
            }

            Func<IQueryable<File>, IOrderedQueryable<File>> orderBy = query => query.OrderBy(entity => entity.FileDetails.Name);

            IEnumerable<FileDtoFetchResponse> response = await _fileService.GetAllFiles(
                pageNumber, 
                pageSize, 
                filter, 
                orderBy
                );

            return response;
        }
        
        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<FileDtoFetchResponse> GetById(Guid id)
        {
            FileDtoFetchResponse response = await _fileService.GetFileByIdAsync(id);

            return response;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<FileDtoCreateResponse> Create([FromBody] FileDtoCreateRequest requestBody)
        {
            FileDtoCreateResponse response = await _fileService.CreateFileAsync(requestBody);

            return response;
        }
        
        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task Update(Guid id, [FromBody] FileDtoUpdateRequest updatedFileDto)
        {
            await _fileService.UpdateFileAsync(id, updatedFileDto);
        }
        
        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task Delete(Guid id)
        {
            await _fileService.DeleteFileAsync(id);
        }

        [Authorize]
        [HttpDelete]
        public async Task Delete(File file)
        {
            await _fileService.DeleteFileAsync(file);
        }
    }
}
