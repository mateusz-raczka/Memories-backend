using MemoriesBackend.Domain.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<UploadFileResult> UploadFileAsync(IFormFile file, string absoluteFolderPath);
        Task<FileContentResult> DownloadFileAsync(string absoluteFilePath);
        Task DeleteFileAsync(string absoluteFilePath);
        Task<Guid> CopyAndPasteFileAsync(string fileAbsolutePath, string destinationFolderAbsolutePath);
        FileStreamResult StreamFile(string absoluteFilePath);
    }
}
