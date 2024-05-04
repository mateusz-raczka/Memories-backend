using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Response;

namespace Memories_backend.Repositories
{
    public interface IFolderRepository : ISQLRepository<Folder>
    {
        Task<Folder> FindRootFolderAsync();
        Task<IEnumerable<FolderHierarchy>> GetFolderHierarchyAsync(Guid id);
    }
}
