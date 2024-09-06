using System.Linq.Expressions;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

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
            await _folderRepository.Save();

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
            return await _folderRepository.GetAll(filter, orderBy, pageNumber, pageSize, asNoTracking).ToListAsync();
        }

        public async Task<Folder> GetFolderByIdAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetById(folderId, asNoTracking);
        }

        public async Task<Folder> GetFolderByIdWithAllRelations(Guid folderId, bool asNoTracking = true)
        {
            var folderWithRelations =  await _folderRepository
                .GetQueryable(asNoTracking)
                .Include(f => f.ChildFolders)
                .Include(f => f.Files)
                .Where(f => f.Id == folderId)
                .FirstOrDefaultAsync();

            if(folderWithRelations == null)
            {
                throw new ApplicationException($"Folder with ID {folderId} was not found");
            }

            return folderWithRelations;
        }

        public async Task<Folder> CreateFolderAsync(Folder folder)
        {
            if (folder.ParentFolderId == null)
                throw new ApplicationException("Folder must have a parent folder.");

            folder.HierarchyId = await GenerateHierarchyId(folder.ParentFolderId);

            var createdFolder = await _folderRepository.Create(folder);
            await _folderRepository.Save();

            return createdFolder;
        }

        public async Task<Folder> GetRootFolderAsync(bool asNoTracking = true)
        {
            var rootFolder = await _folderRepository
                .GetQueryable(asNoTracking)
                .Include(f => f.Files)
                .Include(f => f.ChildFolders)
                .FirstOrDefaultAsync(f => f.ParentFolderId == null);

            if(rootFolder == null)
            {
                throw new ApplicationException("Critical error - root folder was not found");
            }

            return rootFolder;
        }

        public async Task<IEnumerable<Folder>> GetFolderAncestorsAsync(Folder folder, bool asNoTracking = true)
        {
            return await _folderRepository
                .GetQueryable(asNoTracking)
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true)
        {
            var folder = await GetFolderByIdAsync(folderId, asNoTracking);

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
                .Where(f => f.ParentFolderId == parentFolderId)
                .OrderByDescending(f => f.HierarchyId)
                .FirstOrDefaultAsync();
        }

        public async Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId)
        {
            if (parentFolderId == null)
                return HierarchyId.GetRoot();

            var parentFolder = await GetFolderByIdAsync(parentFolderId.Value);
            var lastSibling = await GetFolderLastSiblingAsync(parentFolderId.Value);

            return lastSibling == null
                ? parentFolder.HierarchyId.GetDescendant(null, null)
                : parentFolder.HierarchyId.GetDescendant(lastSibling.HierarchyId, null);
        }
    }
}
