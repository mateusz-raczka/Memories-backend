using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Application.Services
{
    public class FolderAndFileManagementService : IFolderAndFileManagementService
    {
        private readonly IFolderManagementService _folderManagementService;
        private readonly IFileManagementService _fileManagementService;

        public FolderAndFileManagementService(
            IFolderManagementService folderManagementService,
            IFileManagementService fileManagementService
            )
        {
            _fileManagementService = fileManagementService;
            _folderManagementService = folderManagementService;
        }
        public async Task<FoldersAndFiles> CopyAndPasteFoldersAndFilesAsync(
            IEnumerable<Guid> filesIds,
            IEnumerable<Guid> foldersIds,
            Guid targetFolderId
            )
        {
            var pastedFiles = await _fileManagementService.CopyAndPasteFilesAsync(filesIds, targetFolderId);
            var pastedFolders = await _folderManagementService.CopyAndPasteFoldersAsync(foldersIds, targetFolderId);

            var copyAndPasteFoldersAndFilesResult = new FoldersAndFiles
            {
                Folders = pastedFolders,
                Files = pastedFiles,
            };

            return copyAndPasteFoldersAndFilesResult;
        }

        public async Task<FoldersAndFiles> CutAndPasteFoldersAndFilesAsync(
            IEnumerable<Guid> filesIds,
            IEnumerable<Guid> foldersIds,
            Guid targetFolderId
            )
        {
            var pastedFiles = await _fileManagementService.CutAndPasteFilesAsync(filesIds, targetFolderId);
            var pastedFolders = await _folderManagementService.CutAndPasteFoldersAsync(foldersIds, targetFolderId);

            var cutAndPasteFoldersAndFilesResult = new FoldersAndFiles
            {
                Folders = pastedFolders,
                Files = pastedFiles,
            };

            return cutAndPasteFoldersAndFilesResult;
        }
    }
}
