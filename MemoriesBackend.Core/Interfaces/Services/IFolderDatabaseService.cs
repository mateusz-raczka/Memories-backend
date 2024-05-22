using System.Linq.Expressions;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IFolderDatabaseService
    {
        Task<Folder> CreateRootFolderAsync();
        Task<IEnumerable<Folder>> GetAllFoldersAsync(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<Folder, bool>>? filter = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null
            );
        Task<Folder> GetFolderByIdAsync(Guid folderId);
        Task<Folder> CreateFolderAsync(Folder createModel);
        Task<string> GetFolderRelativePathAsync(Guid folderId);
    }
}
