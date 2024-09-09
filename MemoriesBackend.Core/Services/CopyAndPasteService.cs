using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Domain.Models.FileManagement;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    public class CopyAndPasteService : ICopyAndPasteService
    {
        IFileDatabaseService _fileDatabaseService;
        IFileStorageService _fileStorageService;
        IFolderDatabaseService _folderDatabaseService;
        IPathService _pathService;
        ITransactionHandler _transactionHandler;

        public CopyAndPasteService(
            IFileDatabaseService fileDatabaseService,
            IFileStorageService fileStorageService,
            IFolderDatabaseService folderDatabaseService,
            ITransactionHandler transactionHandler,
            IPathService pathService
            ) 
        { 
            _fileDatabaseService = fileDatabaseService;
            _fileStorageService = fileStorageService;
            _folderDatabaseService = folderDatabaseService;
            _transactionHandler = transactionHandler;
            _pathService = pathService;
        }

        public async Task<CopyAndPasteFoldersAndFilesResult> CopyAndPasteFoldersAndFilesAsync(IEnumerable<Guid> filesIds, IEnumerable<Guid> foldersIds, Guid targetFolderId)
        {
            var pastedFiles = await CopyAndPasteFilesAsync(filesIds, targetFolderId);
            var pastedFolders = await CopyAndPasteFoldersAsync(foldersIds, targetFolderId);

            var copyAndPasteFoldersAndFilesResult = new CopyAndPasteFoldersAndFilesResult
            {
                Folders = pastedFolders,
                Files = pastedFiles,
            };

            return copyAndPasteFoldersAndFilesResult;
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
                await CopyAndPasteFileAsync(file, folderPasted.Id);
            }

            foreach (var childFolder in sourceFolder.ChildFolders)
            {
                await CopyAndPasteFolderAsync(childFolder.Id, folderPasted.Id);
            }

            //await _folderRepository.Save();

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


        public async Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<File> filesToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach (var file in filesToCopy)
            {
                var pastedFile = await CopyAndPasteFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            return pastedFiles;
        }

        public async Task<IEnumerable<File>> CopyAndPasteFilesAsync(IEnumerable<Guid> filesIdsToCopy, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            var pastedFiles = new List<File>();

            foreach (var fileId in filesIdsToCopy)
            {
                var file = await _fileDatabaseService.GetFileByIdAsync(fileId);
                var pastedFile = await CopyAndPasteFileAsync(file, targetFolderId);
                pastedFiles.Add(pastedFile);
            }

            return pastedFiles;
        }

        public async Task<File> CopyAndPasteFileAsync(File file, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if (targetFolder == null)
                throw new ApplicationException("Target folder not found");

            return await _transactionHandler.ExecuteAsync(async () =>
            {
                var folderAbsolutePath = await _pathService.GetFolderAbsolutePathAsync(targetFolderId);
                var fileAbsolutePath = await _pathService.GetFileAbsolutePathAsync(file.Id);
                var pastedFileId = await _fileStorageService.CopyAndPasteFileAsync(fileAbsolutePath, folderAbsolutePath);

                var fileCopy = new File
                {
                    Id = pastedFileId,
                    FolderId = targetFolderId,
                    FileDetails = new FileDetails
                    {
                        Id = pastedFileId,
                        Name = file.FileDetails.Name,
                        Size = file.FileDetails.Size,
                        Extension = file.FileDetails.Extension,
                    }
                };

                return await _fileDatabaseService.CreateFileAsync(fileCopy);
            });
        }
    }
}
