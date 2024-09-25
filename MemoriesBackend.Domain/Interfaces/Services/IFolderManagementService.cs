using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderManagementService
    {
        Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId);

        Task<IEnumerable<Folder>> MoveFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId);

        Task DeleteFolderAsync(Guid folderId);
    }
}
