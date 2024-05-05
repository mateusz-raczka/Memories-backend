using Memories_backend.Models.Domain;

namespace Memories_backend.Repositories
{
    public interface IFolderRepository : ISQLRepository<Folder>
    {
        Task<Folder> FindRootFolderAsync();
    }
}
