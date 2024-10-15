using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Repositories
{
    public interface IFolderRepository : IGenericRepository<Folder>
    {
        Task<Folder> GetFolderByIdWithContentAsync(Guid folderId, bool asNoTracking = true);
        Task<Folder?> GetFolderLastSiblingAsync(Guid parentFolderId, bool asNoTracking = true);
        Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true);
        Task<Folder> GetRootFolderAsync(bool asNoTracking = true);
        Task<List<Folder>> GetFolderDescendantsAsync(Folder folder, bool asNoTracking = true);
        Task<List<Folder>> GetFolderDescendantsAsync(Guid folderId, bool asNoTracking = true);
        Task<List<Folder>> GetFoldersByIdsWithContentAsync(IEnumerable<Guid> folderIds, bool asNoTracking = true);
        Task<Folder> GetFolderSubTreeAsync(Guid folderId, bool asNoTracking = true);
    }
}
