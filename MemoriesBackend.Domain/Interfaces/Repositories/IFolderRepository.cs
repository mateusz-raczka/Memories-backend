using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Repositories
{
    public interface IFolderRepository : IGenericRepository<Folder>
    {
        Task<List<Folder>> GetFolderAncestorsAsync(Folder parentFolder);
        Task<List<Folder>> GetFolderAncestorsAsync(Guid parentFolderId);
        Task<Folder> GetRootFolderAsync();
        Task<Folder> GetFolderLastSiblingAsync(Guid parentFolderId);
    }
}
