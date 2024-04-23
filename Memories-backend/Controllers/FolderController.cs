using Memories_backend.Models.DTO.Folder.Response;
using Memories_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memories_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FolderController : ControllerBase
    {
        private readonly IFolderDatabaseService _folderDatabaseService;
        public FolderController(IFolderDatabaseService folderDatabaseService)
        {
            _folderDatabaseService = folderDatabaseService;
        }

        [HttpGet]
        public async Task<IEnumerable<FolderDtoFetchResponse>> GetAll() 
        {
            IEnumerable<FolderDtoFetchResponse> response = await _folderDatabaseService.GetAllFoldersAsync();

            return response;
        }
    }
}
