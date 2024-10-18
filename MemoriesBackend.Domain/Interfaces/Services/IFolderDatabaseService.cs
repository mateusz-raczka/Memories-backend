using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderDatabaseService
    {
        Task<Folder> CreateRootFolderAsync();

        Task<IEnumerable<Folder>> GetAllFoldersAsync(
            Expression<Func<Folder, bool>>? filter = null,
            int? pageNumber = null,
            int? pageSize = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null,
            bool asNoTracking = true
        );

        Task<Folder> GetFolderByIdAsync(Guid folderId, bool asNoTracking = true);

        Task<Folder> CreateFolderAsync(Folder folder);

        Task<Folder> GetRootFolderAsync(bool asNoTracking = true);

        Task<List<Folder>> GetFolderDescendantsAsync(Folder folder, bool asNoTracking = true);

        Task<List<Folder>> GetFolderDescendantsAsync(Guid folderId, bool asNoTracking = true);

        Task<List<Folder>> GetFolderAncestorsAsync(Folder folder, bool asNoTracking = true);

        Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true);

        Task<Folder?> GetFolderLastSiblingAsync(Guid parentFolderId, bool asNoTracking = true);

        Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId);

        HierarchyId GenerateHierarchyId(Folder parentFolder, Folder? childFolderLastSibling);

        Task<Folder> GetFolderByIdWithContentAsync(Guid folderId, bool asNoTracking = true);

        Task<Folder> GetFolderSubTreeAsync(Guid folderId, bool asNoTracking = true);

        Task<Folder> MoveFolderSubTreeAsync(Folder folderSubTreeToMove, Folder targetFolder);

        Task<List<Folder>> GetFoldersSubTreesAsync(IEnumerable<Guid> folderIds, bool asNoTracking = true);

        Task<List<Folder>> GetFoldersWithContentAsync(IEnumerable<Folder> folders, bool asNoTracking = true);

        Task<List<Folder>> MoveFoldersSubTreesAsync(List<Folder> foldersSubTreesToMove, Folder targetFolder);

        Task<Folder> GetFolderWithContentAsync(Folder folder, bool asNoTracking = true);

        Task<FolderWithDescendants> GetFolderByIdWithContentAndDescendants(Guid folderId, bool asNoTracking = true);

        Task<List<Folder>> GetFoldersByIdsWithContentAsync(IEnumerable<Guid> folderIds, bool asNoTracking = true);

        Task DeleteFolderAsync(Guid folderId);

        void UpdateFolderAsync(Folder folder);

        Task SaveAsync();
    }
}
