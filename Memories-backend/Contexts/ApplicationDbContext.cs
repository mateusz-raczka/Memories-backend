using Microsoft.EntityFrameworkCore;
using Memories_backend.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Memories_backend.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityType>().HasData(_activityTypes );

            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.Category).AutoInclude();
            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.FileDetails).AutoInclude();
            modelBuilder.Entity<Models.Domain.File>().Navigation(e => e.Tags).AutoInclude();

            modelBuilder.Entity<Models.Domain.File>()
            .HasOne(f => f.FileDetails)
            .WithOne(fd => fd.File)
            .HasForeignKey<FileDetails>(fd => fd.Id);
        }
    }
}
