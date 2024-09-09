using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models.FileManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileService
    {
        Task<File> AddFileAsync(IFormFile fileData, Guid folderId);
        Task DeleteFileAsync(Guid fileId);
        Task<FileContentResult> DownloadFileAsync(Guid fileId);
        Task<FileStreamResult> StreamFileAsync(Guid fileId);
        Task DeleteFolderAsync(Guid folderId);
    }
}
