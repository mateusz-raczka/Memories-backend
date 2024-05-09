using Memories_backend.Models.Domain;
using Memories_backend.Models.Domain.Interfaces;
using Memories_backend.Models.DTO.Folder.Response;

namespace Memories_backend.Repositories
{
    public interface IFolderRepository : ISQLRepository<Folder>
    {
        Task<List<Folder>> GetFolderAncestorsAsync(Folder parentFolder);
        Task<List<Folder>> GetFolderAncestorsAsync(Guid parentFolderId);
        Task<Folder> GetRootFolderAsync();
        Task<List<Folder>> GetFolderDescendantsAsync(Folder parentFolder);
        Task ChangeFolderSubTreeAsync(Folder oldParentFolder, Folder newParentFolder, Folder folder);
    }
}
