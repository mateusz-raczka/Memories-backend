using System.Linq.Expressions;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Application.Services
{
    public class FolderDatabaseService : IFolderDatabaseService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderDatabaseService(
            IFolderRepository folderRepository
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

        public async Task<Folder> GetRootFolderAsync()
        {
            var rootFolder = await _folderRepository.GetQueryable()
                .Where(f => f.ParentFolderId == null)
                .Include(f => f.ChildFolders)
                .FirstOrDefaultAsync();

            if (rootFolder == null) throw new ApplicationException("Root folder was not found");

            return rootFolder;
        }

        public async Task<IEnumerable<Folder>> GetFolderAncestorsAsync(Folder folder)
        {
            var ancestors = await _folderRepository.GetQueryable()
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();

            return ancestors;
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetById(folderId);

            var ancestors = await _folderRepository.GetQueryable()
                .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
                .OrderBy(f => f.HierarchyId)
                .ToListAsync();

            return ancestors;
        }

        public async Task<Folder> GetFolderLastSiblingAsync(Guid parentFolderId)
        {
            var siblings = await _folderRepository.GetQueryable()
                .Where(f => f.ParentFolderId == parentFolderId)
                .OrderByDescending(f => f.HierarchyId)
                .ToListAsync();

            return siblings.FirstOrDefault();
        }

        public async Task<Folder> CopyFolderAsync(Guid sourceFolderId, Guid targetFolderId)
        {
            var sourceFolder = await _folderRepository.GetById(sourceFolderId);
            var targetFolder = await _folderRepository.GetById(targetFolderId);

            if (sourceFolder == null)
                throw new ApplicationException("Source folder not found");

            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var newFolder = new Folder
            {
                Id = Guid.NewGuid(),
                FolderDetails = sourceFolder.FolderDetails,
                ParentFolderId = targetFolderId,
                HierarchyId = targetFolder.HierarchyId.GetDescendant(null, null),
                Files = sourceFolder.Files,
                ChildFolders = sourceFolder.ChildFolders
            };

            await _folderRepository.Create(newFolder);
            await _folderRepository.Save();

            return newFolder;
        }

        public async Task MoveFolderAsync(Guid sourceFolderId, Guid targetFolderId)
        {
            var sourceFolder = await _folderRepository.GetById(sourceFolderId);
            var targetFolder = await _folderRepository.GetById(targetFolderId);

            if (sourceFolder == null)
                throw new ApplicationException("Source folder not found");

            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var folderHierarchy = await _folderRepository.GetQueryable()
                .Where(f => f.HierarchyId != sourceFolder.HierarchyId && f.HierarchyId.IsDescendantOf(sourceFolder.HierarchyId))
                .ToListAsync();

            foreach (var folder in folderHierarchy)
            {
                folder.OldHierarchyId = folder.HierarchyId;
                folder.HierarchyId = folder.HierarchyId.GetReparentedValue(sourceFolder.HierarchyId, targetFolder.HierarchyId);
            }

            await _folderRepository.Save();
        }

        private async Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId)
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