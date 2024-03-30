using Microsoft.AspNetCore.Mvc;
using Memories_backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace Memories_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class FileSystemController : ControllerBase
    {
        private readonly IFileSystemService _fileIOService;
        public FileSystemController(IFileSystemService fileIOService)
        {
            _fileIOService = fileIOService;
        }

        [HttpDelete("{id:Guid}")]
        public void Delete(Guid id)
        {
            _fileIOService.DeleteFile(id);
        }

        [HttpPost]
        public async Task Upload(byte[] file, Guid id)
        {
            await _fileIOService.UploadFileAsync(file, id);
        }

        [HttpGet("{id:Guid}")]
        public async Task<byte[]> Download(Guid id)
        {
            byte[] response = await _fileIOService.DownloadFileAsync(id);

            return response;
        }
    }
}
