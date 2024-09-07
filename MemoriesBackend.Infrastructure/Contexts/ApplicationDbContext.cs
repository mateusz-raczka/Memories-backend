using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Infrastructure.Contexts;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options)
    {

    }

    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<FileActivity> FileActivities { get; set; }
    public DbSet<FileDetails> FileDetails { get; set; }
    public DbSet<FolderDetails> FolderDetails { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ExtendedIdentityUser> IdentityUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SeedRoles();
        modelBuilder.SeedActivityTypes();

        // Auto include

        modelBuilder.Entity<File>()
            .Navigation(e => e.Category)
            .AutoInclude();
        modelBuilder.Entity<File>()
            .Navigation(e => e.FileDetails)
            .AutoInclude();
        modelBuilder.Entity<File>()
            .Navigation(e => e.Tags)
            .AutoInclude();

        modelBuilder.Entity<Folder>()
            .Navigation(e => e.FolderDetails)
            .AutoInclude();

        // Relations

        modelBuilder.Entity<File>()
            .HasOne(f => f.FileDetails)
            .WithOne(fd => fd.File)
            .HasForeignKey<FileDetails>(fd => fd.Id);

        modelBuilder.Entity<Folder>()
            .HasOne(f => f.FolderDetails)
            .WithOne(fd => fd.Folder)
            .HasForeignKey<FolderDetails>(fd => fd.Id);
    }
}