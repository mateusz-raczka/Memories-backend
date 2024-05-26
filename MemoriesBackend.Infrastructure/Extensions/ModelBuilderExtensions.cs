using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void SeedRoles(this ModelBuilder builder)
    {
        var roles = new List<IdentityRole>
        {
            new(ApplicationRoles.USER),
            new(ApplicationRoles.OWNER),
            new(ApplicationRoles.ADMIN)
        };

        roles.ForEach(role => role.NormalizedName = role.Name.ToUpper());

        builder.Entity<IdentityRole>().HasData(roles);
    }

    public static void SeedActivityTypes(this ModelBuilder builder)
    {
        var activityTypes = new List<ActivityType>
        {
            new() { Id = Guid.NewGuid(), Name = "Edit" },
            new() { Id = Guid.NewGuid(), Name = "Share" },
            new() { Id = Guid.NewGuid(), Name = "Transfer" },
            new() { Id = Guid.NewGuid(), Name = "Create" },
            new() { Id = Guid.NewGuid(), Name = "Delete" },
            new() { Id = Guid.NewGuid(), Name = "Open" }
        };

        builder.Entity<ActivityType>().HasData(activityTypes);
    }
}