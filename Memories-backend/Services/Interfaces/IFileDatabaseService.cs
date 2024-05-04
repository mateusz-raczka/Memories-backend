using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.File.Response;
using System.Linq.Expressions;

namespace Memories_backend.Services.Interfaces
{
    public interface IFileDatabaseService
    {
        Task<FileDtoCreateResponse> CreateFileAsync(FileDtoCreateRequest createModel);
        Task UpdateFileAsync(Guid id, FileDtoUpdateRequest updateModel);
        Task DeleteFileAsync(Guid id);
        Task DeleteFileAsync(Models.Domain.File file);
        Task<FileDtoFetchResponse> GetFileByIdAsync(Guid id);
        Task<IEnumerable<FileDtoFetchResponse>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<Models.Domain.File, bool>>? filter = null,
            Func<IQueryable<Models.Domain.File>, IOrderedQueryable<Models.Domain.File>>? orderBy = null
            );
        Task<bool> FileExistsAsync(Guid id);
    }
}
