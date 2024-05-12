using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.File.Response;
using System.Linq.Expressions;
using File = Memories_backend.Models.Domain.File;

namespace Memories_backend.Services.Interfaces
{
    public interface IFileDatabaseService
    {
        Task<FileDtoCreateResponse> CreateFileAsync(FileDtoCreateRequest createModel);
        Task UpdateFileAsync(Guid id, FileDtoUpdateRequest updateModel);
        Task DeleteFileAsync(Guid id);
        Task DeleteFileAsync(File file);
        Task<FileDtoFetchResponse> GetFileByIdAsync(Guid id);
        Task<IEnumerable<FileDtoFetchResponse>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<File, bool>>? filter = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null
            );
        Task<bool> FileExistsAsync(Guid id);
    }
}
