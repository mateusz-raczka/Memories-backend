using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Infrastructure.Repositories
{
    public interface IFolderRepository : IGenericRepository<Folder>
    {
        Task<List<Folder>> GetFolderAncestorsAsync(Folder parentFolder);
        Task<List<Folder>> GetFolderAncestorsAsync(Guid parentFolderId);
        Task<Folder> GetRootFolderAsync();
        Task<List<Folder>> GetFolderDescendantsAsync(Folder parentFolder);
        Task ChangeFolderSubTreeAsync(Folder oldParentFolder, Folder newParentFolder, Folder folder);
    }
}
