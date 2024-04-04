using Microsoft.EntityFrameworkCore;
using Memories_backend.Models.Domain.Folder.File;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Memories_backend.Utilities.Extensions;
using Memories_backend.Services;
using Memories_backend.Models.Domain.Authorization;
using Memories_backend.Models.Domain.Folder;

namespace Memories_backend.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly Guid _userId;
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileActivity> FileActivities { get; set; }
        public DbSet<ComponentDetails> FileDetails { get; set; }
        public DbSet<Models.Domain.Folder.File.File> Files { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IUserClaimsService userData
            ) : base(options)
        {
            _userId = userData.UserId;
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MarkCreatedItemAsOwnedBy(_userId);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.MarkCreatedItemAsOwnedBy(_userId);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SeedRoles();
            modelBuilder.SeedActivityTypes();

            foreach (var entityOwnedBy in modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType.GetInterface(nameof(IOwnerId)) != null))
            {
                modelBuilder.Entity(entityOwnedBy.ClrType).HasIndex(nameof(IOwnerId.OwnerId));
            }

            modelBuilder.Entity<Models.Domain.Folder.File.File>().HasQueryFilter(x => x.OwnerId == _userId); 

            modelBuilder.Entity<Models.Domain.Folder.File.File>().Navigation(e => e.Category).AutoInclude();
            modelBuilder.Entity<Models.Domain.Folder.File.File>().Navigation(e => e.FileDetails).AutoInclude();
            modelBuilder.Entity<Models.Domain.Folder.File.File>().Navigation(e => e.Tags).AutoInclude();

            modelBuilder.Entity<Models.Domain.Folder.File.File>()
            .HasOne(f => f.FileDetails)
            .WithOne(fd => fd.File)
            .HasForeignKey<ComponentDetails>(fd => fd.Id);

            modelBuilder.Entity<Folder>()
            .HasOne(f => f.FolderDetails)
            .WithOne(fd => fd.Folder)
            .HasForeignKey<ComponentDetails>(fd => fd.Id);
        }
    }
}
