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