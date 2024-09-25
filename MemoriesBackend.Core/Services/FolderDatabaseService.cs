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

        public async Task<Folder> GetFolderByIdWithContent(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderByIdWithContent(folderId, asNoTracking);
        }

        public async Task<Folder> GetFolderByIdWithDetails(Guid folderId, bool asNoTracking = true)
        {
            return await _folderRepository.GetFolderByIdWithDetails(folderId, asNoTracking);
        }

        public async Task<FolderWithDescendants> GetFolderByIdWithContentAndDescendants(Guid folderId, bool asNoTracking = true)
        {
            var folder = await GetFolderByIdWithContent(folderId, asNoTracking);

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

        public async Task SaveAsync()
        {
            await _folderRepository.Save();
        }
    }
}
