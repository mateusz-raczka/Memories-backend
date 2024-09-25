using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<FileUploadedMetaData> UploadFileAsync(IFormFile file, string absoluteFolderPath);
        Task<FileContentResult> DownloadFileAsync(string absoluteFilePath);
        void DeleteFile(string absoluteFilePath);
        Task<Guid> CopyAndPasteFileAsync(string fileAbsolutePath, string destinationFolderAbsolutePath);
        Task<FileStreamResult> StreamFileAsync(string absoluteFilePath);
        Task<Guid> UploadFileChunkAsync(Stream stream, string absoluteFolderPath, Guid fileId);
        Task MergeAndDeleteFileChunksAsync(FileUploadProgress uploadProgress, string absoluteFolderPath);
        Task MergeFileChunksAsync(FileUploadProgress uploadProgress, string absoluteFolderPath);
        Task MoveFileAsync(string fileAbsolutePath, string destinationFolderAbsolutePath);
    }
}
