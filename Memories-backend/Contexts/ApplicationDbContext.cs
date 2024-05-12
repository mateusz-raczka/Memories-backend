using Microsoft.EntityFrameworkCore;
using Memories_backend.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Memories_backend.Utilities.Extensions;
using Memories_backend.Services.Interfaces;
using Memories_backend.Models.Domain.Interfaces;

namespace Memories_backend.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IUserClaimsService _userClaimsService;
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileActivity> FileActivities { get; set; }
        public DbSet<FileDetails> FileDetails { get; set; }
        public DbSet<FolderDetails> FolderDetails { get; set; }
        public DbSet<Models.Domain.File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IUserClaimsService userClaimsService
            ) : base(options)
        {
            _userClaimsService = userClaimsService;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MarkCreatedItemAsOwnedBy(_userClaimsService.UserClaimsValues.UserId);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.MarkCreatedItemAsOwnedBy(_userClaimsService.UserClaimsValues.UserId);
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

            modelBuilder.Entity<Models.Domain.File>().HasQueryFilter(x => x.OwnerId == _userClaimsService.UserClaimsValues.UserId);
            modelBuilder.Entity<Folder>().HasQueryFilter(x => x.OwnerId == _userClaimsService.UserClaimsValues.UserId);

            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.Category).AutoInclude();
            modelBuilder.Entity<Models.Domain.File>()
                .Navigation(e => e.FileDetails)
                .AutoInclude();
            modelBuilder.Entity<Models.Domain.File>()
                .Navigation(e => e.Tags)
                .AutoInclude();

            modelBuilder.Entity<Folder>()
                .Navigation(e => e.FolderDetails)
                .AutoInclude();
            modelBuilder.Entity<Folder>()
                .Navigation(e => e.Files)
                .AutoInclude();

            modelBuilder.Entity<Models.Domain.File>()
                .HasOne(f => f.FileDetails)
                .WithOne(fd => fd.File)
                .HasForeignKey<FileDetails>(fd => fd.Id);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.FolderDetails)
                .WithOne(fd => fd.Folder)
                .HasForeignKey<FolderDetails>(fd => fd.Id);

        }
    }
}
