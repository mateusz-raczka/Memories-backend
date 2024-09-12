using MemoriesBackend.Domain.Models.FileStorage;
using MemoriesBackend.Domain.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<FileUploadedResult> UploadFileAsync(IFormFile file, string absoluteFolderPath);
        Task<FileContentResult> DownloadFileAsync(string absoluteFilePath);
        void DeleteFile(string absoluteFilePath);
        Task<Guid> CopyAndPasteFileAsync(string fileAbsolutePath, string destinationFolderAbsolutePath);
        FileStreamResult StreamFile(string absoluteFilePath);
        Task UploadFileChunkAsync(Stream stream, FileChunkMetaData fileChunkMetaData, string absoluteFolderPath);
    }
}
