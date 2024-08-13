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

        public FolderDatabaseService(
            IGenericRepository<Folder> folderRepository,
            IFileDatabaseService fileDatabaseService
        )
        {
            _folderRepository = folderRepository;
        }

        public async Task<Folder> CreateRootFolderAsync()
        {
            var folder = new Folder();

            folder.HierarchyId = await GenerateHierarchyId(null);

            var createdFolder = await _folderRepository.Create(folder);

            await _folderRepository.Save();

            return createdFolder;
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<Folder, bool>>? filter = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null
        )
        {
            var folders = await _folderRepository.GetAll(
                pageNumber,
                pageSize,
                filter,
                orderBy
            );

            return folders;
        }


        public async Task<Folder> GetFolderByIdAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetById(folderId);

            if (folder == null)
                throw new KeyNotFoundException("Failed to fetch - There was no folder found with given id.");

            return folder;
        }

        public async Task<Folder> CreateFolderAsync(Folder folder)
        {
            if (folder.ParentFolderId == null) throw new ApplicationException("Folder must have parent folder");

            var parentFolderId = folder.ParentFolderId;

            folder.HierarchyId = await GenerateHierarchyId(parentFolderId);

            var createdFolder = await _folderRepository.Create(folder);

            await _folderRepository.Save();

            return createdFolder;
        }

        public async Task<Folder> CopyFolderAsync(Guid folderId)
        {
            var folderCopy = await _folderRepository
                .GetQueryable()
                .Include(f => f.ChildFolders)
                .Include(f => f.Files)
                .Include(f => f.FolderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == folderId);

            folderCopy.Id = Guid.NewGuid();
            folderCopy.FolderDetails.Id = Guid.NewGuid();
            folderCopy.FolderDetails.Name = folderCopy.FolderDetails.Name + " COPY test";
            
            return folderCopy;
        }

        public async Task<Folder> GetRootFolderAsync()
        {
            var rootFolder = await _folderRepository
                .GetQueryable()
                .Where(f => f.ParentFolderId == null)
                .Include(f => f.ChildFolders)
                .FirstOrDefaultAsync();

            if (rootFolder == null) throw new ApplicationException("Root folder was not found");

            return rootFolder;
        }

        public async Task<Folder> GetFolderByIdWithRelations(Guid folderId)
        {
            var folder = await _folderRepository
                .GetQueryable()
                .Where(f => f.Id == folderId)
                .Include(f => f.ChildFolders)
                .FirstOrDefaultAsync();

            return folder;
        }

        public async Task<IEnumerable<Folder>> GetFolderAncestorsAsync(Folder folder)
        {
            var ancestors = await _folderRepository
                .GetQueryable()
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();

            return ancestors;
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetById(folderId);

            var ancestors = await _folderRepository
                .GetQueryable()
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();

            return ancestors;
        }

        public async Task<Folder> GetFolderLastSiblingAsync(Guid parentFolderId)
        {
            var siblings = await _folderRepository
                .GetQueryable()
                .Where(f => f.ParentFolderId == parentFolderId)
                .OrderByDescending(f => f.HierarchyId)
                .ToListAsync();

            return siblings.FirstOrDefault();
        }

        public async Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId)
        {
            if (parentFolderId == null) return HierarchyId.GetRoot();

            var parentFolder = await _folderRepository.GetById(parentFolderId.Value);
            var parentHierarchyId = parentFolder.HierarchyId;

            var lastSibling = await GetFolderLastSiblingAsync(parentFolderId.Value);

            var childHierarchyId = lastSibling == null
                ? parentHierarchyId.GetDescendant(null, null)
                : parentHierarchyId.GetDescendant(lastSibling.HierarchyId, null);

            return childHierarchyId;
        }
    }
}