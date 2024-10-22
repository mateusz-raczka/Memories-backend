using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;

namespace MemoriesBackend.Application.Services
{
    public class FolderManagementService : IFolderManagementService
    {
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly IFileManagementService _fileManagementService;
        private readonly IPathService _pathService;
        private readonly IFolderStorageService _folderStorageService;

        public FolderManagementService(
            IFileManagementService fileManagementService,
            IFolderDatabaseService folderDatabaseService,
            IPathService pathService,
            IFolderStorageService folderStorageService
            )
        {
            _fileManagementService = fileManagementService;
            _folderDatabaseService = folderDatabaseService;
            _pathService = pathService;
            _folderStorageService = folderStorageService;
        }

        public async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId)
        {
            var foldersToCopy = await _folderDatabaseService.GetFoldersByIdsWithContentAsync(folderIds);

            var pastedFolders = await CopyAndPasteFoldersAsync(foldersToCopy, targetFolderId);

            return pastedFolders;
        }

        public async Task<IEnumerable<Folder>> MoveFoldersAsync(IEnumerable<Guid> foldersIdsToMove, Guid targetFolderId)
        {
            var foldersToMove = await _folderDatabaseService.GetAllFoldersAsync(filter: f=> foldersIdsToMove.Contains(f.Id));
            var targetFolder = await _folderDatabaseService.GetFolderByIdWithContentAsync(targetFolderId);

            if (!foldersToMove.Any()) 
            {
                throw new ApplicationException("Failed to move folder/s - no folder was found");
            }

            if(targetFolder == null)
            {
                throw new ApplicationException("Failed to move folder/s - target folder was not found");
            }

            await MoveFoldersInStorageAsync(foldersIdsToMove, targetFolderId);

            var movedFolders = await _folderDatabaseService.MoveFoldersSubTreesAsync(foldersToMove, targetFolder);

            await _folderDatabaseService.SaveAsync();

            return movedFolders;
        }

        public async Task DeleteFolderAsync(Guid folderId)
        {
            var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
            if (folder == null)
            {
                throw new ApplicationException($"Cannot delete folder with Id {folderId} - it was not found");
            }

            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync(folderId);

            await _folderDatabaseService.DeleteFolderAsync(folderId);
            await _folderDatabaseService.SaveAsync();
            await _folderStorageService.DeleteFolderAsync(absoluteFolderPath);
        }

        private async Task MoveFoldersInStorageAsync(IEnumerable<Guid> sourceFoldersIds, Guid targetFolderId)
        {
            var targetFolderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);

            foreach (var sourceFolderId in sourceFoldersIds)
            {
                var sourceFolderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(sourceFolderId);
                await _folderStorageService.MoveFolderAsync(sourceFolderAbsolutePath, targetFolderAbsolutePath);
            }
        }

        private async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Folder> foldersToCopy, Guid targetFolderId)
        {
            var pastedFolders = new List<Folder>();

            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);

            foreach (var folderToCopy in foldersToCopy)
            {
                var pastedFolder = await CopyAndPasteFolderAsync(folderToCopy, targetFolder);
                pastedFolders.Add(pastedFolder);
            }

            return pastedFolders;
        }

        private async Task<Folder> CopyAndPasteFolderAsync(Folder sourceFolder, Folder targetFolder)
        {
            var newFolderId = Guid.NewGuid();

            var folderToPaste = new Folder
            {
                Id = newFolderId,
                ParentFolderId = targetFolder.Id,
                FolderDetails = new FolderDetails
                {
                    Id = newFolderId,
                    Name = sourceFolder.FolderDetails.Name
                },
            };

            var folderPasted = await _folderDatabaseService.CreateFolderAsync(folderToPaste);

            await _folderDatabaseService.SaveAsync();

            if (sourceFolder.Files.Any())
            {
                await _fileManagementService.CopyAndPasteFilesAsync(sourceFolder.Files, folderPasted.Id);
            }

            if (sourceFolder.ChildFolders.Any())
            {
                var childFoldersWithAllContent = await _folderDatabaseService.GetFoldersWithContentAsync(sourceFolder.ChildFolders);

                await CopyAndPasteFoldersAsync(childFoldersWithAllContent, folderPasted.Id);
            }

            return folderPasted;
        }
    }
}
