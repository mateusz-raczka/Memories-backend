using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Folder> CopyAndPasteFolderAsync(Guid sourceFolderId, Guid targetFolderId)
        {
            var sourceFolder = await _folderDatabaseService.GetFolderByIdWithContent(sourceFolderId);

            if (sourceFolder == null)
            {
                throw new ApplicationException($"Source folder with ID {sourceFolderId} not found.");
            }

            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);

            if (targetFolder == null)
            {
                throw new ApplicationException($"Target folder with ID {targetFolderId} not found.");
            }

            var newFolderId = Guid.NewGuid();

            var folderToPaste = new Folder
            {
                Id = newFolderId,
                ParentFolderId = targetFolderId,
                FolderDetails = new FolderDetails
                {
                    Id = newFolderId,
                    Name = sourceFolder.FolderDetails.Name
                },
            };

            folderToPaste.HierarchyId = await _folderDatabaseService.GenerateHierarchyId(targetFolderId);

            var folderPasted = await _folderDatabaseService.CreateFolderAsync(folderToPaste);

            await _folderDatabaseService.SaveAsync();

            if (sourceFolder.Files.Any())
            {
                await _fileManagementService.CopyAndPasteFilesAsync(sourceFolder.Files, folderPasted.Id);
            }

            if (sourceFolder.ChildFolders.Any())
            {
                await CopyAndPasteFoldersAsync(sourceFolder.ChildFolders, folderPasted.Id);
            }

            return folderPasted;
        }

        public async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId)
        {
            var foldersToCopy = await _folderDatabaseService.GetAllFoldersAsync(
                f => folderIds.Contains(f.Id)
                );

            var pastedFolders = new List<Folder>();

            if (foldersToCopy.Any())
            {
                foreach(var folderToCopy in foldersToCopy)
                {
                    var pastedFolder = await CopyAndPasteFolderAsync(folderToCopy.Id, targetFolderId);
                    pastedFolders.Add(pastedFolder);
                }
            }

            return pastedFolders;
        }

        public async Task<IEnumerable<Folder>> MoveFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId)
        {
            var pastedFolders = new List<Folder>();

            foreach (var folderId in folderIds)
            {
                var pastedFolder = await MoveFolderAsync(folderId, targetFolderId);
                pastedFolders.Add(pastedFolder);
            }

            await MoveFoldersInStorageAsync(folderIds, targetFolderId);

            await _folderDatabaseService.SaveAsync();

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

        private async Task MoveFoldersInStorageAsync(IEnumerable<Guid> sourceFoldersIds, Guid targetFolderId)
        {
            var targetFolderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);
   
            foreach(var sourceFolderId in sourceFoldersIds)
            {
                await MoveFolderInStorageAsync(sourceFolderId, targetFolderAbsolutePath);
            }
        }

        private async Task MoveFolderInStorageAsync(Guid sourceFolderId, string targetFolderAbsolutePath)
        {
            var sourceFolderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(sourceFolderId);

            await _folderStorageService.MoveFolderAsync(sourceFolderAbsolutePath, targetFolderAbsolutePath);
        }

        private async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Folder> foldersToCopy, Guid targetFolderId)
        {
            var pastedFolders = new List<Folder>();

            if (foldersToCopy.Any())
            {
                foreach (var folderToCopy in foldersToCopy)
                {
                    var pastedFolder = await CopyAndPasteFolderAsync(folderToCopy.Id, targetFolderId);
                    pastedFolders.Add(pastedFolder);
                }
            }

            return pastedFolders;
        }

        private async Task<Folder> MoveFolderAsync(Guid sourceFolderId, Guid targetFolderId)
        {
            var sourceFolder = await _folderDatabaseService.GetFolderByIdAsync(sourceFolderId);

            if (sourceFolder == null)
            {
                throw new ApplicationException($"Source folder with ID {sourceFolderId} not found.");
            }

            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);

            if (targetFolder == null)
            {
                throw new ApplicationException($"Target folder with ID {targetFolderId} not found.");
            }

            sourceFolder.ParentFolderId = targetFolderId;

            _folderDatabaseService.UpdateFolderAsync(sourceFolder);

            return sourceFolder;
        }
    }
}
