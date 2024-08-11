using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<Guid> UploadFileAsync(IFormFile file, string absoluteFolderPath);
        Task<FileContentResult> DownloadFileAsync(string absoluteFilePath);
        Task DeleteFileAsync(string absoluteFilePath);
    }
}
