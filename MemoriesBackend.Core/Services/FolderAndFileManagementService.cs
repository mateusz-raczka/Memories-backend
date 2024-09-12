using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models.FolderAndFileManagement;

namespace MemoriesBackend.Application.Services
{
    public class FolderAndFileManagementService : IFolderAndFileManagementService
    {
        IFolderManagementService _folderManagementService;
        IFileManagementService _fileManagementService;

        public FolderAndFileManagementService(
            IFolderManagementService folderManagementService,
            IFileManagementService fileManagementService
            ) 
        {
            _fileManagementService = fileManagementService;
            _folderManagementService = folderManagementService;
        }
        public async Task<CopyAndPasteFoldersAndFilesResult> CopyAndPasteFoldersAndFilesAsync(
            IEnumerable<Guid> filesIds,
            IEnumerable<Guid> foldersIds,
            Guid targetFolderId
            )
        {
            var pastedFiles = await _fileManagementService.CopyAndPasteFilesAsync(filesIds, targetFolderId);
            var pastedFolders = await _folderManagementService.CopyAndPasteFoldersAsync(foldersIds, targetFolderId);

            var copyAndPasteFoldersAndFilesResult = new CopyAndPasteFoldersAndFilesResult
            {
                Folders = pastedFolders,
                Files = pastedFiles,
            };

            return copyAndPasteFoldersAndFilesResult;
        }
    }
}
