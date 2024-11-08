﻿using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using File = MemoriesBackend.Domain.Entities.File;

public class FileRepository : GenericRepository<File>, IFileRepository
{
    public FileRepository(ApplicationDbContext context, IUserContextService userContextService)
        : base(context, userContextService)
    {
    }

    public async Task<File> GetFileByIdWithDetailsAsync(Guid fileId, bool asNoTracking = true)
    {
        var file = await GetQueryable(asNoTracking)
            .Include(f => f.FileDetails)
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
        {
            throw new ApplicationException("Failed to fetch - There was no file found with the given id.");
        }

        return file;
    }

    public async Task<List<File>> GetFilesByIdsWithDetailsAsync(IEnumerable<Guid> fileIds, bool asNoTracking = true)
    {
        var files = await GetQueryable(asNoTracking)
            .Where(f => fileIds.Contains(f.Id))
            .Include(f => f.FileDetails)
            .ToListAsync();

        if (files == null || !files.Any())
        {
            throw new ApplicationException("Failed to fetch - No files found with the provided IDs.");
        }

        return files;
    }

}
