using Microsoft.AspNetCore.Mvc;

namespace Memories_backend.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<Guid> UploadFileAsync(IFormFile file, Guid folderId);
        Task<FileContentResult> DownloadFileAsync(Guid id, Guid folderId);
        Task DeleteFileAsync(Guid id, Guid folderId);
    }
}
