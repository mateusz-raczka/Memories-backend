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
            var pastedFolders = new List<Folder>();

            foreach (var folderId in foldersIdsToMove)
            {
                var pastedFolder = await MoveFolderAsync(folderId, targetFolderId);
                pastedFolders.Add(pastedFolder);
            }

            await _folderDatabaseService.SaveAsync();

            await MoveFoldersInStorageAsync(foldersIdsToMove, targetFolderId);

            return pastedFolders;
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

        private async Task<IEnumerable<Folder>> MoveFoldersAsync(IEnumerable<Folder> foldersToMove, Guid targetFolderId)
        {
            IEnumerable<Folder> pastedFolders = [];

            if (foldersToMove.Any())
            {
                var foldersIdsToMove = foldersToMove.Select(f => f.Id).ToList();

                pastedFolders = await MoveFoldersAsync(foldersIdsToMove, targetFolderId);
            }

            return pastedFolders;
        }

        private async Task MoveFoldersInStorageAsync(IEnumerable<Guid> sourceFoldersIds, Guid targetFolderId)
        {
            var targetFolderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);

            var sourceFoldersAbsolutePathsTasks = sourceFoldersIds.Select(_pathService.GetFolderAbsolutePathAsync);

            var sourceFoldersAbsolutePaths = await Task.WhenAll(sourceFoldersAbsolutePathsTasks);

            foreach (var sourceFolderAbsolutePath in sourceFoldersAbsolutePaths)
            {
                await _folderStorageService.MoveFolderAsync(sourceFolderAbsolutePath, targetFolderAbsolutePath);
            }
        }

        private async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Folder> foldersToCopy, Guid targetFolderId)
        {
            var pastedFolders = new List<Folder>();

            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);

            if (foldersToCopy.Any())
            {
                foreach (var folderToCopy in foldersToCopy)
                {
                    var pastedFolder = await CopyAndPasteFolderAsync(folderToCopy, targetFolder);
                    pastedFolders.Add(pastedFolder);
                }
            }

            return pastedFolders;
        }

        private async Task<Folder> MoveFolderAsync(Guid sourceFolderId, Guid targetFolderId)
        {
            var sourceFolder = await _folderDatabaseService.GetFolderByIdWithContentAsync(sourceFolderId);
            if (sourceFolder == null)
            {
                throw new ApplicationException($"Source folder with ID {sourceFolderId} not found.");
            }

            var targetFolder = await _folderDatabaseService.GetFolderByIdWithContentAsync(targetFolderId);
            if (targetFolder == null)
            {
                throw new ApplicationException($"Target folder with ID {targetFolderId} not found.");
            }

            var sourceFolderSubTree = await _folderDatabaseService.GetFolderSubTreeAsync(sourceFolderId);

            await _folderDatabaseService.MoveFolderSubTreeAsync(sourceFolderSubTree, targetFolder);

            await _folderDatabaseService.SaveAsync();

            return sourceFolderSubTree;
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
