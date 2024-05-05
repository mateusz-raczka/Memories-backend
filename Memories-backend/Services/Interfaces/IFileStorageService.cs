using Microsoft.AspNetCore.Mvc;

namespace Memories_backend.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<Guid> UploadFileAsync(IFormFile file, Guid folderId);
        Task<FileContentResult> DownloadFileAsync(Guid id);
        Task DeleteFileAsync(Guid id);
    }
}
