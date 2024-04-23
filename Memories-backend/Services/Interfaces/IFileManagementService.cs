using Memories_backend.Models.DTO.File.Request;

namespace Memories_backend.Services.Interfaces
{
    public interface IFileManagementService
    {
        Task<FileDtoCreateResponse> AddFileToDatabaseAndStorageAsync(IFormFile fileData, Guid folderId);
    }
}
