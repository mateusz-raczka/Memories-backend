using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MemoriesBackend.Application.Services
{
    public class FolderDatabaseService : IFolderDatabaseService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderDatabaseService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<Folder> CreateRootFolderAsync()
        {
            var folder = new Folder();

            var createdFolder = await CreateFolderAsync(folder);

            await SaveAsync();

            return createdFolder;
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync(
            Expression<Func<Folder, bool>>? filter = null,
            int? pageNumber = null,
            int? pageSize = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null,
            bool asNoTracking = true
        )
        {
            return await _folderRepository
                .GetAll(filter, orderBy, pageNumber, pageSize, asNoTracking)
                .ToListAsync();
        }

        public async Task<Folder> GetFolderByIdAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetById(folderId, asNoTracking);
        }

        public async Task<Folder> GetFolderByIdWithContentAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderByIdWithContentAsync(folderId, asNoTracking);
        }

        public async Task<FolderWithDescendants> GetFolderByIdWithContentAndDescendants(Guid folderId, bool asNoTracking = true)
        {
            var folder = await GetFolderByIdWithContentAsync(folderId, asNoTracking);

            var descendants = await GetFolderDescendantsAsync(folder, asNoTracking);

            var folderWithDescendants = new FolderWithDescendants
            {
                Folder = folder,
                Descendants = descendants,
            };

            return folderWithDescendants;
        }

        public async Task<Folder> CreateFolderAsync(Folder folder)
        {
            if(folder.HierarchyId == null)
                folder.HierarchyId = await GenerateHierarchyId(folder.ParentFolderId);

            if(folder == null)
            {
                throw new ApplicationException("Failed to create - folder is null");
            }

            var createdFolder = await _folderRepository.Create(folder);

            return createdFolder;
        }

        public async Task<Folder> GetRootFolderAsync(bool asNoTracking = true)
        {
            return await _folderRepository.GetRootFolderAsync(asNoTracking);
        }

        public async Task<List<Folder>> GetFolderDescendantsAsync(Folder folder, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderDescendantsAsync(folder, asNoTracking);
        }

        public async Task<List<Folder>> GetFolderDescendantsAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderDescendantsAsync(folderId, asNoTracking);
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Folder folder, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderAncestorsAsync(folder.Id, asNoTracking);
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderAncestorsAsync(folderId, asNoTracking);
        }

        public async Task<Folder?> GetFolderLastSiblingAsync(Guid parentFolderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderLastSiblingAsync(parentFolderId, asNoTracking);
        }

        public async Task<List<Folder>> GetFoldersByIdsWithContentAsync(IEnumerable<Guid> folderIds, bool asNoTracking = true)
        {
            return await _folderRepository.GetFoldersByIdsWithContentAsync(folderIds, asNoTracking);
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

        public HierarchyId GenerateHierarchyId(Folder parentFolder, Folder? childFolderLastSibling)
        {
            if (parentFolder == null)
                return HierarchyId.GetRoot();

            return childFolderLastSibling == null
                ? parentFolder.HierarchyId.GetDescendant(null, null)
                : parentFolder.HierarchyId.GetDescendant(childFolderLastSibling.HierarchyId, null);
        }

        public async Task DeleteFolderAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetById(folderId);

            if (folder == null)
            {
                throw new ApplicationException($"Failed to delete - folder with Id {folderId} was not found");
            }

            var folderAncestors = await GetFolderAncestorsAsync(folder);

            foreach ( var ancestor in folderAncestors)
            {
                _folderRepository.Delete(ancestor);
            }
        }

        public void UpdateFolderAsync(Folder folder)
        {
            if (folder == null)
            {
                throw new ApplicationException("Failed to update folder - folder does not exist");
            }

            _folderRepository.Update(folder);
        }

        public async Task<Folder> GetFolderWithContentAsync(Folder folder, bool asNoTracking = true)
        {
            if (folder == null)
            {
                throw new ApplicationException("Failed to get folder content - folder does not exist");
            }

            var folderWithContent = await _folderRepository.GetFolderByIdWithContentAsync(folder.Id, asNoTracking);

            return folderWithContent;
        }

        public async Task<List<Folder>> GetFoldersWithContentAsync(IEnumerable<Folder> folders, bool asNoTracking = true)
        {
            if(!folders.Any() || folders == null)
            {
                throw new ApplicationException("Failed to get folders content - provided list of folders is empty or null");
            }

            var foldersIds = folders.Select(f => f.Id);

            var foldersWithContent = await _folderRepository.GetFoldersByIdsWithContentAsync(foldersIds, asNoTracking);

            return foldersWithContent;
        }

        public async Task<Folder> GetFolderSubTreeAsync(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderSubTreeAsync(folderId, asNoTracking);
        }

        public async Task<List<Folder>> GetFoldersSubTreesAsync(IEnumerable<Guid> folderIds, bool asNoTracking = true)
        {
            var foldersSubTrees = new List<Folder>();

            foreach(var folderId in folderIds)
            {
                var folderSubTree = await GetFolderSubTreeAsync(folderId);

                foldersSubTrees.Add(folderSubTree);
            }

            return foldersSubTrees;
        }

        public async Task<List<Folder>> MoveFoldersSubTreesAsync(List<Folder> foldersSubTreesToMove, Folder targetFolder)
        {
            if (!foldersSubTreesToMove.Any())
            {
                throw new ApplicationException("Failed to move folders sub trees - there are no provided folders to move");
            }

            var foldersSubTreesInTheSameDirectoryAsTargetFolder = foldersSubTreesToMove
                .Where(f => f.ParentFolderId == targetFolder.ParentFolderId)
                .ToList();

            if (foldersSubTreesInTheSameDirectoryAsTargetFolder.Any())
            {
                var targetFolderParent = await GetFolderByIdWithContentAsync((Guid)targetFolder.ParentFolderId);

                Folder? targetFolderLastSibling = targetFolderParent.ChildFolders.OrderByDescending(f => f.HierarchyId)
                                                            .FirstOrDefault(f => foldersSubTreesInTheSameDirectoryAsTargetFolder.Contains(f) && f.Id != targetFolder.Id);

                targetFolder.HierarchyId = GenerateHierarchyId(targetFolderParent, targetFolderLastSibling);

                UpdateFolderAsync(targetFolder);
            }

            foreach(var folderSubTreeToMove in foldersSubTreesToMove)
            {
                ChangeFolderSubTreeParent(folderSubTreeToMove, targetFolder);
            }

            return foldersSubTreesToMove;
        }

        public async Task<Folder> MoveFolderSubTreeAsync(Folder folderSubTreeToMove, Folder targetFolder)
        {
            if (folderSubTreeToMove.ParentFolderId == targetFolder.ParentFolderId)
            {
                var targetFolderParent = await GetFolderByIdWithContentAsync((Guid)targetFolder.ParentFolderId);

                Folder? targetFolderLastSibling = targetFolderParent.ChildFolders.OrderByDescending(f => f.HierarchyId)
                                                           .FirstOrDefault(f => f.Id != targetFolder.Id && f.Id != folderSubTreeToMove.Id);

                targetFolder.HierarchyId = GenerateHierarchyId(targetFolderParent, targetFolderLastSibling);

                UpdateFolderAsync(targetFolder);
            }

            ChangeFolderSubTreeParent(folderSubTreeToMove, targetFolder);

            return folderSubTreeToMove;
        }

        private void ChangeFolderSubTreeParent(Folder folderSubTreeToMove, Folder targetFolder)
        {
            ChangeFolderParent(folderSubTreeToMove, targetFolder);

            var childFolders = folderSubTreeToMove.ChildFolders.ToList();

            foreach (var childFolder in childFolders)
            {
                ChangeFolderSubTreeParent(childFolder, folderSubTreeToMove);
            }
        }

        public async Task SaveAsync()
        {
            await _folderRepository.Save();
        }

        private void ChangeFolderParent(Folder sourceFolder, Folder targetFolder)
        {
            Folder? sourceFolderLastSibling = targetFolder.ChildFolders.OrderByDescending(f => f.HierarchyId)
                                                           .FirstOrDefault(f => f.Id != sourceFolder.Id);

            sourceFolder.ParentFolderId = targetFolder.Id;

            sourceFolder.HierarchyId = GenerateHierarchyId(targetFolder, sourceFolderLastSibling);

            targetFolder.ChildFolders.Add(sourceFolder);

            UpdateFolderAsync(sourceFolder);
        }
    }
}
