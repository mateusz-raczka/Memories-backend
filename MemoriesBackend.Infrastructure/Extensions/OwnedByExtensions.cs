using MemoriesBackend.Domain.Interfaces.Models;
using MemoriesBackend.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Extensions;

public static class OwnedByExtensions
{
    /// <summary>
    ///     This is called in the overridden SaveChanges in the application's DbContext
    ///     Its job is to call the SetOwnedBy on entities that have it and are being created
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userId"></param>
    public static void MarkCreatedItemAsOwnedBy(this DbContext context, UserContext userContext)
    {
        foreach (var entityEntry in context.ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added))
            if (entityEntry.Entity is IOwnerId entityToMark)
                entityToMark.SetOwnerId(userContext.UserData.Id);
    }
}