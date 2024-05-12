using Memories_backend.Contexts;
using Memories_backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Memories_backend.Repositories
{
    public class FolderRepository : SQLRepository<Folder>, IFolderRepository
    {
        public FolderRepository(ApplicationDbContext context) : base(context) { }

        public Task ChangeFolderSubTreeAsync(Folder oldParentFolder, Folder newParentFolder, Folder folder)
        {
            throw new NotImplementedException();
        }

        public Task<List<Folder>> GetFolderDescendantsAsync(Folder parentFolderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Folder> GetRootFolderAsync() =>
            await _context.Folders.FirstOrDefaultAsync(f => f.FolderId == null);

        public async Task<List<Folder>> GetFolderAncestorsAsync(Folder parentFolder)
        {
            var parentHierarchyId = parentFolder.HierarchyId;

            var ancestors = await _dbSet
                .Where(node => parentHierarchyId.IsDescendantOf(node.HierarchyId))
                .ToListAsync();

            return ancestors;
        }

        public async Task<List<Folder>> GetFolderAncestorsAsync(Guid parentFolderId)
        {
            var parentFolder = await GetById(parentFolderId);

            var parentHierarchyId = parentFolder.HierarchyId;

            var ancestors = await _dbSet
                .Where(node => parentHierarchyId.IsDescendantOf(node.HierarchyId))
                .ToListAsync();

            return ancestors;
        }
    }
}
