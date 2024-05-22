using System.Linq.Expressions;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Infrastructure.Repositories;
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

        public async Task<Folder> FindRootFolderAsync()
        {
            return await _folderRepository.GetRootFolderAsync();
        }

        public async Task<string> GetFolderRelativePathAsync(Guid folderId)
        {
            var folderHierarchy = await _folderRepository.GetFolderAncestorsAsync(folderId);

            var folderIds = folderHierarchy.Select(x => x.Id);

            var path = string.Join("/", folderIds);

            return path;
        }

        private async Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId)
        {
            if (parentFolderId == null) return HierarchyId.GetRoot();

            var parentFolder = await _folderRepository.GetById(parentFolderId.Value);

            var parentHierarchyId = parentFolder.HierarchyId;

            var childHierarchyId = parentHierarchyId.GetDescendant(null, null);

            return childHierarchyId;
        }
    }
}