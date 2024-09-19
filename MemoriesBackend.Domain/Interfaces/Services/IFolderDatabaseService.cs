using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models;
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
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null,
            bool asNoTracking = true
        );

        Task<Folder> GetFolderByIdAsync(Guid folderId, bool asNoTracking = true);

        Task<Folder> CreateFolderAsync(Folder folder);

        Task<Folder> GetRootFolderAsync(bool asNoTracking = true);

        Task<List<Folder>> GetFolderAncestorsAsync(Folder folder, bool asNoTracking = true);

        Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true);

        Task<Folder?> GetFolderLastSiblingAsync(Guid parentFolderId, bool asNoTracking = true);

        Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId);

        Task<Folder> GetFolderByIdWithRelations(Guid folderId, bool asNoTracking = true);

        Task<FolderWithAncestors> GetFolderByIdWithRelationsAndAncestors(Guid folderId, bool asNoTracking = true);

        Task DeleteFolderAsync(Guid folderId);

        Task SaveAsync();
    }
}
