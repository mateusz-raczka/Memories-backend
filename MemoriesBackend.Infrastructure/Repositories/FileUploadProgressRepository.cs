using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Repositories
{
    public class FileUploadProgressRepository : GenericRepository<FileUploadProgress>, IFileUploadProgressRepository
    {
        public FileUploadProgressRepository(ApplicationDbContext context, IUserContextService userContextService)
        : base(context, userContextService)
        {
        }

        public async Task<FileUploadProgress> GetFileUploadProgressByIdWithRelationsAsync(Guid fileUploadProgressId, bool asNoTracking = true)
        {
            return await GetQueryable(asNoTracking)
                 .Include(fu => fu.FileChunks)
                 .Where(fu => fu.Id == fileUploadProgressId)
                 .FirstOrDefaultAsync(fu => fu.Id == fileUploadProgressId);
        }
    }
}
