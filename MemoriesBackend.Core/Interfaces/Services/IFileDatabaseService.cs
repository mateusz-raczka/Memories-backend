using System.Linq.Expressions;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IFileDatabaseService
    {
        Task<File> CreateFileAsync(File file);
        Task UpdateFileAsync(Guid id, File file);
        Task DeleteFileAsync(Guid id);
        Task DeleteFileAsync(File file);
        Task<File> GetFileByIdAsync(Guid id);
        Task<IEnumerable<File>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<File, bool>>? filter = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null
            );
    }
}
