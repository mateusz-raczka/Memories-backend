using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Repositories;

public class FolderRepository : GenericRepository<Folder>, IFolderRepository
{
    public FolderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Folder> GetRootFolderAsync()
    {
        return await _context.Folders.FirstOrDefaultAsync(f => f.ParentFolderId == null);
    }

    public async Task<List<Folder>> GetFolderAncestorsAsync(Folder folder)
    {
        var ancestors = await _dbSet
            .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
            .OrderBy(f => f.HierarchyId)
            .ToListAsync();

        return ancestors;
    }

    public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId)
    {
        var folder = await GetById(folderId);

        var ancestors = await _dbSet
            .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
            .OrderBy(f => f.HierarchyId)
            .ToListAsync();

        return ancestors;
    }

    public async Task<Folder> GetFolderLastSiblingAsync(Guid parentFolderId)
    {
        var folder = await _dbSet
            .Where(f => f.ParentFolderId == parentFolderId)
            .OrderByDescending(f => f.HierarchyId)
            .FirstOrDefaultAsync();

        return folder;
    }
    public override async Task<Folder> GetById(Guid id)
    {
        var entity = await _dbSet
        .Where(f => f.Id == id)
        .Include(f => f.ChildFolders)
        .FirstOrDefaultAsync();

        if (entity == null) throw new ApplicationException("Entity was not found");

        return entity;
    }
}