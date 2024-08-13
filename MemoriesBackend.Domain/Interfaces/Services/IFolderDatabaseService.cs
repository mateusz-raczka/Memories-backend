using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MemoriesBackend.Domain.Interfaces.Services
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
        Task<Folder> GetFolderByIdWithRelations(Guid folderId);
        Task<Folder> CreateFolderAsync(Folder createModel);
        Task<Folder> GetRootFolderAsync();
        Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId);
        Task<Folder> GetFolderLastSiblingAsync(Guid folderId);
        Task<Folder> CopyFolderAsync(Guid folderId);
        Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId);
    }
}
