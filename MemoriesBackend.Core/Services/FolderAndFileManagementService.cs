using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;
using File = MemoriesBackend.Domain.Entities.File;

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
            IEnumerable<File> pastedFiles = [];
            IEnumerable<Folder> pastedFolders = [];

            if (foldersIds.Any())
            {
                pastedFolders = await _folderManagementService.CopyAndPasteFoldersAsync(foldersIds, targetFolderId);
            }

            if (filesIds.Any())
            {
                pastedFiles = await _fileManagementService.CopyAndPasteFilesAsync(filesIds, targetFolderId);
            }

            if(!filesIds.Any() && !foldersIds.Any())
            {
                throw new ApplicationException("Failed to copy - there were no files or folders provided");
            }

            var copyAndPasteFoldersAndFilesResult = new FoldersAndFiles
            {
                Folders = pastedFolders,
                Files = pastedFiles,
            };

            return copyAndPasteFoldersAndFilesResult;
        }

        public async Task<FoldersAndFiles> MoveFoldersAndFilesAsync(
            IEnumerable<Guid> filesIds,
            IEnumerable<Guid> foldersIds,
            Guid targetFolderId
            )
        {
            IEnumerable<File> pastedFiles = [];
            IEnumerable<Folder> pastedFolders = [];

            if (filesIds.Any() )
            {
                pastedFiles = await _fileManagementService.MoveFilesAsync(filesIds, targetFolderId);
            }

            if(foldersIds.Any() )
            {
                pastedFolders = await _folderManagementService.MoveFoldersAsync(foldersIds, targetFolderId);
            }

            if(!filesIds.Any() && !foldersIds.Any())
            {
                throw new ApplicationException("Failed to move - there were no files or folders provided");
            }

            var cutAndPasteFoldersAndFilesResult = new FoldersAndFiles
            {
                Folders = pastedFolders,
                Files = pastedFiles,
            };

            return cutAndPasteFoldersAndFilesResult;
        }

        public async Task DeleteFoldersAndFilesAsync(
            IEnumerable<Guid> filesIds,
            IEnumerable<Guid> foldersIds
            )
        {
            foreach(var fileId in filesIds)
            {
                await _fileManagementService.DeleteFileAsync(fileId);
            }

            foreach(var folderId in foldersIds )
            {
                await _folderManagementService.DeleteFolderAsync(folderId);
            }
        }
    }
}
