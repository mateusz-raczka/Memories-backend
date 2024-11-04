using MemoriesBackend.Domain.Entities;
using System.Linq.Expressions;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileDatabaseService
    {
        Task<File> CreateFileAsync(File file);
        Task DeleteFileAsync(Guid fileId);
        void DeleteFileAsync(File file);
        Task<File> GetFileByIdAsync(Guid fileId, bool asNoTracking = true);
        Task<File> GetFileByIdWithDetailsAsync(Guid fileId, bool asNoTracking = true);

        Task<List<File>> GetFilesByIdsWithDetailsAsync(IEnumerable<Guid> fileIds, bool asNoTracking = true);
        Task<IEnumerable<File>> GetAllFilesAsync(
            Expression<Func<File, bool>> filter,
            int? pageNumber = null,
            int? pageSize = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null,
            bool asNoTracking = true
        );
        Task BatchCreateFilesAsync(IEnumerable<File> files);
        void UpdateFile(File file);
        void PatchFileDetails(FileDetails fileDetails, params Expression<Func<FileDetails, object>>[] updatedProperties);
        Task SwitchFileStar(Guid fileId);
        Task SaveAsync();
    }
}
