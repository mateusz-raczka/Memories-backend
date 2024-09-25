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
    public DbSet<FileUploadProgress> FileUploadProgress { get; set; }
    public DbSet<FileChunk> FileChunks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SeedRoles();
        modelBuilder.SeedActivityTypes();

        // Cascade delete configuration for 1:1

        modelBuilder.Entity<File>()
                .HasOne(e => e.FileDetails)
                .WithOne(e => e.File)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Folder>()
            .HasOne(e => e.FolderDetails)
            .WithOne(e => e.Folder)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Folder>()
           .HasMany(f => f.Files)
           .WithOne(f => f.Folder)
           .HasForeignKey(f => f.FolderId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}