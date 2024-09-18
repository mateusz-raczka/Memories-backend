using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderAndFileManagementService
    {
        Task<FoldersAndFiles> CopyAndPasteFoldersAndFilesAsync(IEnumerable<Guid> filesIds, IEnumerable<Guid> foldersIds, Guid targetFolderId);
        Task<FoldersAndFiles> CutAndPasteFoldersAndFilesAsync(IEnumerable<Guid> filesIds, IEnumerable<Guid> foldersIds, Guid targetFolderId);
    }
}
