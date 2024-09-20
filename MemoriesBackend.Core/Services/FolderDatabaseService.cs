using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MemoriesBackend.Application.Services
{
    public class FolderDatabaseService : IFolderDatabaseService
    {
        private readonly IGenericRepository<Folder> _folderRepository;

        public FolderDatabaseService(IGenericRepository<Folder> folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<Folder> CreateRootFolderAsync()
        {
            var folder = new Folder
            {
                HierarchyId = await GenerateHierarchyId(null)
            };

            var createdFolder = await _folderRepository.Create(folder);

            await SaveAsync();

            return createdFolder;
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<Folder, bool>>? filter = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null,
            bool asNoTracking = true
        )
        {
            return await _folderRepository
                .GetAll(filter, orderBy, pageNumber, pageSize, asNoTracking)
                .Include(f => f.FolderDetails)
                .ToListAsync();
        }

        public async Task<Folder> GetFolderByIdAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetById(folderId, asNoTracking);
        }

        public async Task<Folder> GetFolderByIdWithRelations(Guid folderId, bool asNoTracking = true)
        {
            var folderWithRelations = await _folderRepository
                .GetQueryable(asNoTracking)
                .Include(f => f.FolderDetails)
                .Include(f => f.ChildFolders)
                .Include(f => f.Files)
                .AsSplitQuery()
                .Where(f => f.Id == folderId)
                .FirstOrDefaultAsync();

            if (folderWithRelations == null)
            {
                throw new ApplicationException($"Folder with ID {folderId} was not found");
            }

            return folderWithRelations;
        }

        public async Task<FolderWithAncestors> GetFolderByIdWithRelationsAndAncestors(Guid folderId, bool asNoTracking = true)
        {
            var folder = await GetFolderByIdWithRelations(folderId, asNoTracking);

            var ancestors = await GetFolderAncestorsAsync(folder, asNoTracking);

            var folderWithAncestors = new FolderWithAncestors
            {
                Folder = folder,
                Ancestors = ancestors,
            };

            return folderWithAncestors;
        }

        public async Task<Folder> CreateFolderAsync(Folder folder)
        {
            if (folder.ParentFolderId == null)
                throw new ApplicationException("Folder must have a parent folder.");

            if(folder.HierarchyId == null)
                folder.HierarchyId = await GenerateHierarchyId(folder.ParentFolderId);

            var createdFolder = await _folderRepository.Create(folder);

            return createdFolder;
        }

        public async Task<Folder> GetRootFolderAsync(bool asNoTracking = true)
        {
            var rootFolder = await _folderRepository
                .GetQueryable(asNoTracking)
                .Include(f => f.FolderDetails)
                .Include(f => f.Files)
                .Include(f => f.ChildFolders)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.ParentFolderId == null);

            if (rootFolder == null) 
            {
                throw new ApplicationException("Critical error - root folder was not found");
            }

            return rootFolder;
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Folder folder, bool asNoTracking = true)
        {
            if (folder == null)
            {
                throw new ApplicationException("Failed to get folder's ancestors - folder does not exist");
            }

            return await _folderRepository
                .GetQueryable(asNoTracking)
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true)
        {
            var folder = await GetFolderByIdAsync(folderId, asNoTracking);

            if(folder == null)
            {
                throw new ApplicationException("Failed to get folder's ancestors - folder does not exist");
            }

            return await _folderRepository
                .GetQueryable(asNoTracking)
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();
        }

        public async Task<Folder?> GetFolderLastSiblingAsync(Guid parentFolderId, bool asNoTracking = true)
        {
            return await _folderRepository
                .GetQueryable(asNoTracking)
                .Include(f => f.FolderDetails)
                .Where(f => f.ParentFolderId == parentFolderId)
                .OrderByDescending(f => f.HierarchyId)
                .FirstOrDefaultAsync();
        }

        public async Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId)
        {
            if (parentFolderId == null)
                return HierarchyId.GetRoot();

            var parentFolder = await GetFolderByIdAsync(parentFolderId.Value);

            if(parentFolder == null)
            {
                throw new ApplicationException("Failed to generate hierarchy id - parent folder was not found");
            }

            var lastSibling = await GetFolderLastSiblingAsync(parentFolderId.Value);

            return lastSibling == null
                ? parentFolder.HierarchyId.GetDescendant(null, null)
                : parentFolder.HierarchyId.GetDescendant(lastSibling.HierarchyId, null);
        }

        public async Task DeleteFolderAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetById(folderId);
            if (folder == null)
            {
                throw new ApplicationException($"Failed to delete - folder with Id {folderId} was not found");
            }

            await _folderRepository.Delete(folderId);
        }

        public void UpdateFolderAsync(Folder folder)
        {
            if (folder == null)
            {
                throw new ApplicationException("Failed to update folder - folder does not exist");
            }

            _folderRepository.Update(folder);
        }

        public async Task SaveAsync()
        {
            await _folderRepository.Save();
        }
    }
}
