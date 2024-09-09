using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models.FileManagement;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface ICopyAndPasteService
    {
        Task<Folder> CopyAndPasteFolderAsync(Guid sourceFolderId, Guid targetFolderId);
        Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId);
        Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId);
        Task<CopyAndPasteFoldersAndFilesResult> CopyAndPasteFoldersAndFilesAsync(IEnumerable<Guid> filesIds, IEnumerable<Guid> foldersIds, Guid targetFolderId);
        Task<File> CopyAndPasteFileAsync(File file, Guid targetFolderId);
    }
}
