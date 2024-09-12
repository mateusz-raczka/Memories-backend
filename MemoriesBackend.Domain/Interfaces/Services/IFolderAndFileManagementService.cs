using MemoriesBackend.Domain.Models.FolderAndFileManagement;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderAndFileManagementService
    {
        Task<CopyAndPasteFoldersAndFilesResult> CopyAndPasteFoldersAndFilesAsync(IEnumerable<Guid> filesIds, IEnumerable<Guid> foldersIds, Guid targetFolderId);
    }
}
