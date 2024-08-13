using MemoriesBackend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileManagementSystemService
    {
        Task<File> AddFileAsync(IFormFile fileData, Guid folderId);
        Task DeleteFileAsync(Guid fileId);
        Task<FileContentResult> DownloadFileAsync(Guid fileId);
        Task<Folder> CopyAndPasteFolderAsync(Guid sourceFolderId, Guid targetFolderId);
        Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<Guid> filesIdToCopy, Guid targetFolderId);
    }
}
