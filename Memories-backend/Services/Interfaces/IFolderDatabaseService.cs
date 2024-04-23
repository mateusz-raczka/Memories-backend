using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Response;

namespace Memories_backend.Services.Interfaces
{
    public interface IFolderDatabaseService
    {
        Task<FolderDtoCreateResponse> CreateRootFolderAsync();
        Task<Folder> FindRootFolderAsync();
        Task<IEnumerable<FolderDtoFetchResponse>> GetAllFoldersAsync();
    }
}
