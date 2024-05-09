using Memories_backend.Models.Domain;
using Memories_backend.Utilities.Authentication.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Memories_backend.Utilities.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedRoles(this ModelBuilder builder)
        {
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole(CustomUserRoles.USER),
                new IdentityRole(CustomUserRoles.OWNER),
                new IdentityRole(CustomUserRoles.ADMIN)
            };

            roles.ForEach(role => role.NormalizedName = role.Name.ToUpper());

            builder.Entity<IdentityRole>().HasData(roles);
        }

        public static void SeedActivityTypes(this ModelBuilder builder)
        {
            List<ActivityType> activityTypes = new List<ActivityType>()
            {
                new ActivityType() { Id = Guid.NewGuid(), Name = "Edit"},
                new ActivityType() { Id = Guid.NewGuid(), Name = "Share"},
                new ActivityType() { Id = Guid.NewGuid(), Name = "Transfer"},
                new ActivityType() { Id = Guid.NewGuid(), Name = "Create"},
                new ActivityType() { Id = Guid.NewGuid(), Name = "Delete"},
                new ActivityType() { Id = Guid.NewGuid(), Name = "Open"},
            };

            builder.Entity<ActivityType>().HasData(activityTypes);
        }
    }
}
