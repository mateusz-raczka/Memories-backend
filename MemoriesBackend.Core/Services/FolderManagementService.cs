using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;

namespace MemoriesBackend.Application.Services
{
    public class FolderManagementService : IFolderManagementService
    {
        IFolderDatabaseService _folderDatabaseService;
        IFileManagementService _fileManagementService;

        public FolderManagementService(
            IFileManagementService fileManagementService,
            IFolderDatabaseService folderDatabaseService
            ) 
        {
            _fileManagementService = fileManagementService;
            _folderDatabaseService = folderDatabaseService;
        }

        public async Task<Folder> CopyAndPasteFolderAsync(Guid sourceFolderId, Guid targetFolderId)
        {
            var sourceFolder = await _folderDatabaseService.GetFolderByIdWithAllRelations(sourceFolderId);

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
                HierarchyId = await _folderDatabaseService.GenerateHierarchyId(targetFolderId)
            };

            var folderPasted = await _folderDatabaseService.CreateFolderAsync(folderToPaste);

            foreach (var file in sourceFolder.Files)
            {
                await _fileManagementService.CopyAndPasteFileAsync(file, folderPasted.Id);
            }

            foreach (var childFolder in sourceFolder.ChildFolders)
            {
                await CopyAndPasteFolderAsync(childFolder.Id, folderPasted.Id);
            }

            return folderPasted;
        }

        public async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
            {
                throw new ApplicationException("Target folder not found");
            }

            var pastedFolders = new List<Folder>();

            foreach (var folderId in folderIds)
            {
                var pastedFolder = await CopyAndPasteFolderAsync(folderId, targetFolderId);
                pastedFolders.Add(pastedFolder);
            }

            return pastedFolders;
        }
    }
}
