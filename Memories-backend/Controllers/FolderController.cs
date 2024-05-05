using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Request;
using Memories_backend.Models.DTO.Folder.Response;
using Memories_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Memories_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderDatabaseService _folderDatabaseService;
        public FolderController(IFolderDatabaseService folderDatabaseService)
        {
            _folderDatabaseService = folderDatabaseService;
        }

        [HttpGet]
        public async Task<IEnumerable<FolderDtoFetchAllResponse>> GetAll(
            int? pageNumber,
            int? pageSize,
            string? filterName = null
            ) 
        {
            Expression<Func<Folder, bool>> filter = null;

            if (!string.IsNullOrEmpty(filterName))
            {
                filter = entity => entity.FolderDetails.Name.Contains(filterName);
            }

            // Hardcoded orderby name
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>> orderBy = query => query.OrderBy(entity => entity.FolderDetails.Name);

            IEnumerable<FolderDtoFetchAllResponse> response = await _folderDatabaseService.GetAllFoldersAsync(
                pageNumber,
                pageSize,
                filter,
                orderBy
                );

            return response;
        }

        [HttpGet("{id:Guid}")]
        public async Task<FolderDtoFetchByIdResponse> GetById(Guid id)
        {
            FolderDtoFetchByIdResponse response = await _folderDatabaseService.GetFolderByIdAsync(id);

            return response;
        }

        [HttpPost]
        public async Task<FolderDtoCreateResponse> Create([FromBody] FolderDtoCreateRequest createModel)
        {
            FolderDtoCreateResponse response = await _folderDatabaseService.CreateFolderAsync(createModel);

            return response;
        }
    }
}
