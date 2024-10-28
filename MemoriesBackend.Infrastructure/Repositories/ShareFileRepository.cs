using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Models;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Contexts;

namespace MemoriesBackend.Infrastructure.Repositories;

public class ShareFileRepository : GenericRepository<ShareFile>
{
    public ShareFileRepository(ApplicationDbContext context, IUserContextService userContextService)
    : base(context, userContextService)
    {
    }

    public override async Task<ShareFile> GetById(Guid id, bool asNoTracking = true)
    {
        return new ShareFile();
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
