using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Models;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Infrastructure.Contexts;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly IUserContextService _userContext;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IUserContextService userContext
    ) : base(options)
    {
        _userContext = userContext;
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

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var userContext = _userContext.Current;

        this.MarkCreatedItemAsOwnedBy(userContext);

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        var userContext = _userContext.Current;

        this.MarkCreatedItemAsOwnedBy(userContext);

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SeedRoles();
        modelBuilder.SeedActivityTypes();

        foreach (var entityOwnedBy in modelBuilder.Model.GetEntityTypes()
                     .Where(x => x.ClrType.GetInterface(nameof(IOwnerId)) != null))
            modelBuilder.Entity(entityOwnedBy.ClrType).HasIndex(nameof(IOwnerId.OwnerId));

        modelBuilder.Entity<File>().HasQueryFilter(x => x.OwnerId == _userContext.Current.UserData.Id);
        modelBuilder.Entity<Folder>().HasQueryFilter(x => x.OwnerId == _userContext.Current.UserData.Id);

        modelBuilder.Entity<File>().Navigation(e => e.Category).AutoInclude();
        modelBuilder.Entity<File>()
            .Navigation(e => e.FileDetails)
            .AutoInclude();
        modelBuilder.Entity<File>()
            .Navigation(e => e.Tags)
            .AutoInclude();

        modelBuilder.Entity<Folder>()
            .Navigation(e => e.FolderDetails)
            .AutoInclude();
        modelBuilder.Entity<Folder>()
            .Navigation(e => e.Files)
            .AutoInclude();

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