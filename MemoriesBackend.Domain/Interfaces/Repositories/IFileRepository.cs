using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Interfaces.Repositories
{
    public interface IFileRepository : IGenericRepository<File> 
    {
        Task<File> GetFileByIdWithDetailsAsync(Guid fileId, bool asNoTracking = true);
    }
}
