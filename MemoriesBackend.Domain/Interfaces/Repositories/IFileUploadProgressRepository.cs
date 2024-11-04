using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Repositories
{
    public interface IFileUploadProgressRepository : IGenericRepository<FileUploadProgress>
    {
        Task<FileUploadProgress> GetFileUploadProgressByIdWithRelationsAsync(Guid fileUploadProgressId, bool asNoTracking = true);
    }
}
