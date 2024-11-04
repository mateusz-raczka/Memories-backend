using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Models;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Repositories;

public class ShareFileRepository : GenericRepository<ShareFile>
{
    public ShareFileRepository(ApplicationDbContext context, IUserContextService userContextService)
    : base(context, userContextService)
    {
    }

    public override async Task<ShareFile> GetById(Guid id, bool asNoTracking = true)
    {
        var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();

        var queryWithAppliedOwnershipFilter = ApplyOwnershipFilter(query);

        var shareFile = queryWithAppliedOwnershipFilter
            .Include(sf => sf.File)
                .ThenInclude(f => f.FileDetails)
            .FirstOrDefault(sf => sf.Id == id);

        if (shareFile == null)
        {
            return null;
        }

        return shareFile;
    }

    public IQueryable<ShareFile> ApplyOwnershipFilter(IQueryable<ShareFile> query)
    {
        if (typeof(IOwned).IsAssignableFrom(typeof(ShareFile)))
        {
            var currentUserId = _userContextService.Current.UserData.Id;
            query = query.Where(sf => sf.OwnerId == currentUserId || sf.SharedForUserId == currentUserId);
        }
        return query;
    }
}
