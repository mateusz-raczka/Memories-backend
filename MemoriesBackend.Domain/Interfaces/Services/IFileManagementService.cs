﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileManagementService
    {
        Task<File> AddFileAsync(IFormFile fileData, Guid folderId);
        Task DeleteFileAsync(Guid fileId);
        Task<FileContentResult> DownloadFileAsync(Guid fileId);
        Task<FileStreamResult> StreamFileAsync(Guid fileId);
        Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId);
        Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId);
        Task<File> CopyAndPasteFileAsync(File file, Guid targetFolderId);
        Task<File?> AddFileUsingChunksAsync(Stream stream, string fileName, int chunkIndex, int totalChunks, Guid folderId, Guid fileId);
        Task<IEnumerable<File>> CutAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId);
        Task<IEnumerable<File>> CutAndPasteFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId);
        Task<File> CutAndPasteFileAsync(File file, Guid targetFolderId);
    }
}
