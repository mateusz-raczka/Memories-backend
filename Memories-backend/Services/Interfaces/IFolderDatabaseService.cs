using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Request;
using Memories_backend.Models.DTO.Folder.Response;
using System.Linq.Expressions;

namespace Memories_backend.Services.Interfaces
{
    public interface IFolderDatabaseService
    {
        Task<FolderDtoCreateResponse> CreateRootFolderAsync();
        Task<IEnumerable<FolderDtoFetchAllResponse>> GetAllFoldersAsync(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<Folder, bool>>? filter = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null
            );
        Task<FolderDtoFetchByIdResponse> GetFolderByIdAsync(Guid folderId);
        Task<bool> FolderExistsAsync(Guid folderId);
        Task<FolderDtoCreateResponse> CreateFolderAsync(FolderDtoCreateRequest createModel);
        Task<string> GetFolderRelativePathAsync(Guid folderId);
    }
}
