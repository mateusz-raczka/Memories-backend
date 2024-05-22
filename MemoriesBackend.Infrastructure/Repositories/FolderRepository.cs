using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Repositories;

public class FolderRepository : GenericRepository<Folder>, IFolderRepository
{
    public FolderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task ChangeFolderSubTreeAsync(Folder oldParentFolder, Folder newParentFolder, Folder folder)
    {
        throw new NotImplementedException();
    }

    public Task<List<Folder>> GetFolderDescendantsAsync(Folder parentFolderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Folder> GetRootFolderAsync()
    {
        return await _context.Folders.FirstOrDefaultAsync(f => f.ParentFolderId == null);
    }

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