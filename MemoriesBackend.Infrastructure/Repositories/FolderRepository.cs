﻿using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class FolderRepository : GenericRepository<Folder>, IFolderRepository
{
    public FolderRepository(ApplicationDbContext context, IUserContextService userContextService)
        : base(context, userContextService)
    {
    }

    public async Task<Folder> GetFolderByIdWithContentAsync(Guid folderId, bool asNoTracking = true)
    {
        return await GetQueryable(asNoTracking)
            .Include(folder => folder.ChildFolders)
                .ThenInclude(childFolder => childFolder.FolderDetails)
            .Include(folder => folder.Files)
                .ThenInclude(file => file.FileDetails)
            .Include(folder => folder.FolderDetails)
            .AsSplitQuery()
            .FirstOrDefaultAsync(folder => folder.Id == folderId);
    }

    public async Task<Folder?> GetFolderLastSiblingAsync(Guid parentFolderId, bool asNoTracking = true)
    {
        return await GetQueryable(asNoTracking)
            .Include(folder => folder.FolderDetails)
            .Where(folder => folder.ParentFolderId == parentFolderId)
            .OrderByDescending(folder => folder.HierarchyId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Folder>> GetFolderAncestorsAsync(Guid folderId, bool asNoTracking = true)
    {
        var folder = await GetById(folderId, asNoTracking);

        if (folder == null)
        {
            throw new ApplicationException("Failed to get folder's ancestors - folder does not exist");
        }

        return await GetQueryable(asNoTracking)
            .Include(f => f.FolderDetails)
            .Where(f => f.HierarchyId.IsDescendantOf(folder.HierarchyId))
            .OrderBy(f => f.HierarchyId)
            .ToListAsync();
    }

    public async Task<Folder> GetRootFolderAsync(bool asNoTracking = true)
    {
        var rootFolder = await GetQueryable(asNoTracking)
            .Include(folder => folder.Files)
                .ThenInclude(file => file.FileDetails)
            .Include(folder => folder.ChildFolders)
                .ThenInclude(childFolder => childFolder.FolderDetails)
            .Include(folder => folder.FolderDetails)
            .AsSplitQuery()
            .FirstOrDefaultAsync(folder => folder.ParentFolderId == null);

        if (rootFolder == null)
        {
            throw new ApplicationException("Critical error - root folder was not found");
        }

        return rootFolder;
    }

    public async Task<Folder> GetFolderSubTreeAsync(Guid folderId, bool asNoTracking = true)
    {
        var folder = await GetQueryable(asNoTracking)
            .Include(folder => folder.ChildFolders)
            .Include(folder => folder.FolderDetails)
            .AsSplitQuery()
            .FirstOrDefaultAsync(folder => folder.Id == folderId);

        if (folder == null)
        {
            throw new ApplicationException("Failed to get folder tree - folder in tree does not exist");
        }

        var getChildFoldersTasks = folder.ChildFolders.Select(f => GetFolderSubTreeAsync(f.Id, asNoTracking));

        var childFolders = await Task.WhenAll(getChildFoldersTasks);

        folder.ChildFolders = childFolders.OrderByDescending(f=> f.HierarchyId).ToList();

        return folder;
    }

    public async Task<List<Folder>> GetFolderDescendantsAsync(Folder folder, bool asNoTracking = true)
    {
        if (folder == null)
        {
            throw new ApplicationException("Failed to get folder's ancestors - folder does not exist");
        }

        return await GetQueryable(asNoTracking)
            .Include(f => f.FolderDetails)
            .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
            .OrderBy(f => f.HierarchyId)
            .ToListAsync();
    }
    public async Task<List<Folder>> GetFolderDescendantsAsync(Guid folderId, bool asNoTracking = true)
    {
        var folder = await GetById(folderId, asNoTracking);

        if (folder == null)
        {
            throw new ApplicationException("Failed to get folder's ancestors - folder does not exist");
        }

        return await GetQueryable(asNoTracking)
            .Include(f => f.FolderDetails)
            .Where(f => folder.HierarchyId.IsDescendantOf(f.HierarchyId))
            .OrderBy(f => f.HierarchyId)
            .ToListAsync();
    }

    public async Task<List<Folder>> GetFoldersByIdsWithContentAsync(IEnumerable<Guid> folderIds, bool asNoTracking = true)
    {
        var folders = await GetQueryable(asNoTracking)
            .Where(f => folderIds.Contains(f.Id))
            .Include(folder => folder.ChildFolders)
                .ThenInclude(childFolder => childFolder.FolderDetails)
            .Include(folder => folder.Files)
                .ThenInclude(file => file.FileDetails)
            .Include(folder => folder.FolderDetails)
            .AsSplitQuery()
            .ToListAsync();

        if (folders == null || !folders.Any())
        {
            throw new ApplicationException("Failed to fetch - No folders found with the provided IDs.");
        }

        return folders;
    }
}
