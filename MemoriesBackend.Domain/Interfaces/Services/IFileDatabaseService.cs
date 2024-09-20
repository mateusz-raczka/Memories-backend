﻿using System.Linq.Expressions;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileDatabaseService
    {
        Task<File> CreateFileAsync(File file);
        Task DeleteFileAsync(Guid id);
        Task DeleteFileAsync(File file);
        Task<File> GetFileByIdAsync(Guid id, bool asNoTracking = true);
        Task<IEnumerable<File>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<File, bool>>? filter = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null,
            bool asNoTracking = true
            );
        Task BatchCreateFilesAsync(IEnumerable<File> files);
        Task SaveAsync();
    }
}
