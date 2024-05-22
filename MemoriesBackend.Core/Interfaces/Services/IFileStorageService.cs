using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<Guid> UploadFileAsync(IFormFile file, Guid folderId);
        Task<FileContentResult> DownloadFileAsync(Guid id);
        Task DeleteFileAsync(Guid id);
    }
}
