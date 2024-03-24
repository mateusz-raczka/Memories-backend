using Microsoft.EntityFrameworkCore;
using Memories_backend.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Memories_backend.Utilities.Authorization;
using Memories_backend.Utilities.Extensions;
using Memories_backend.Utilities.Authorization.DataAuthorize;

namespace Memories_backend.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly string _userId;
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileActivity> FileActivities { get; set; }
        public DbSet<FileDetails> FileDetails { get; set; }
        public DbSet<Models.Domain.File> Files { get; set; }
        public DbSet<Tag> Tags { get; set; }

        private List<ActivityType> _activityTypes = new List<ActivityType>()
        {
            new ActivityType() { Id = Guid.NewGuid(), Name = "Edit"},
            new ActivityType() { Id = Guid.NewGuid(), Name = "Share"},
            new ActivityType() { Id = Guid.NewGuid(), Name = "Transfer"},
            new ActivityType() { Id = Guid.NewGuid(), Name = "Create"},
            new ActivityType() { Id = Guid.NewGuid(), Name = "Delete"},
            new ActivityType() { Id = Guid.NewGuid(), Name = "Open"},
        };

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options, 
            IGetClaimsProvider userData
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

            foreach (var entityOwnedBy in modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType.GetInterface(nameof(IOwnerId)) != null))
            {
                modelBuilder.Entity(entityOwnedBy.ClrType).HasIndex(nameof(IOwnerId.OwnerId));
            }

            // Check for owner of entities
            modelBuilder.Entity<Models.Domain.File>().HasQueryFilter(x => x.OwnerId.ToString() == _userId);

            // Seed data to ActivityType table
            modelBuilder.Entity<ActivityType>().HasData(_activityTypes );

            // Automatically include tables when fetching
            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.Category).AutoInclude();
            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.FileDetails).AutoInclude();
            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.Tags).AutoInclude();

            // Define 1:1 relationship
            modelBuilder.Entity<Models.Domain.File>()
            .HasOne(f => f.FileDetails)
            .WithOne(fd => fd.File)
            .HasForeignKey<FileDetails>(fd => fd.Id);
        }
    }
}
