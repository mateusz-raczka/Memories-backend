using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderManagementService
    {
        Task<Folder> CopyAndPasteFolderAsync(Guid sourceFolderId, Guid targetFolderId);
        Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId);
        Task<Folder> CutAndPasteFolderAsync(Guid sourceFolderId, Guid targetFolderId);
        Task<IEnumerable<Folder>> CutAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId);
    }
}
