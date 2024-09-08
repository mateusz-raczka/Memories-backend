using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Domain.Models.FileManagement;
using MemoriesBackend.Domain.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    internal sealed class FileManagementSystemService : IFileManagementSystemService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly ITransactionHandler _transactionHandler;
        private readonly IPathService _pathService;
        private readonly IGenericRepository<Folder> _folderRepository;
        private readonly IFolderStorageService _folderStorageService;

        public FileManagementSystemService(
            IFileStorageService fileStorageService,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService,
            ITransactionHandler transactionHandler,
            IPathService pathService,
            IGenericRepository<Folder> folderRepository,
            IFolderStorageService folderStorageService
            )
        {
            _fileStorageService = fileStorageService;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
            _transactionHandler = transactionHandler;
            _pathService = pathService;
            _folderRepository = folderRepository;
            _folderStorageService = folderStorageService;
        }

        public async Task<File> AddFileAsync(IFormFile fileData, Guid folderId)
        {
            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync( folderId );
            UploadFileResult uploadedFile = null;

            return await _transactionHandler.ExecuteAsync(async () =>
            {
                var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
                if (folder == null) throw new ApplicationException($"Folder with the given ID {folderId} - does not exist.");

                uploadedFile = await _fileStorageService.UploadFileAsync(fileData, absoluteFolderPath);

                if(uploadedFile == null)
                {
                    throw new ApplicationException("Failed to upload file to file storage");
                }

                var file = new File
                {
                    Id = uploadedFile.Id,
                    FolderId = folderId,
                    FileDetails = new FileDetails
                    {
                        Id = uploadedFile.Id,
                        Name = fileData.FileName,
                        Size = fileData.Length,
                        Extension = Path.GetExtension(fileData.FileName)
                    }
                };

                var createdFile = await _fileDatabaseService.CreateFileAsync(file);

                return createdFile;

            }, async () => await UploadFileRollbackAsync(uploadedFile, absoluteFolderPath));
        }

        public async Task<FileStreamResult> StreamFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);
            return _fileStorageService.StreamFile(absoluteFilePath);
        }

        public async Task DeleteFolderAsync(Guid folderId)
        {
            var folder = await _folderDatabaseService.GetFolderByIdAsync(folderId);
            if(folder == null)
            {
                throw new ApplicationException($"Cannot delete folder with Id {folderId} - it was not found");
            }

            var absoluteFolderPath = await _pathService.GetFolderAbsolutePathAsync(folderId);

            await _folderDatabaseService.DeleteFolderAsync(folderId);
            await _folderStorageService.DeleteFolderAsync(absoluteFolderPath);
        }

        public async Task DeleteFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);

            await _transactionHandler.ExecuteAsync(async () =>
            {
                await _fileDatabaseService.DeleteFileAsync(fileId);
                await _fileStorageService.DeleteFileAsync(absoluteFilePath);
            });
        }

        public async Task<FileContentResult> DownloadFileAsync(Guid fileId)
        {
            var absoluteFilePath = await _pathService.GetFileAbsolutePathAsync(fileId);

            return await _fileStorageService.DownloadFileAsync(absoluteFilePath);
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

                await _folderRepository.Save();

                return folderPasted;
        }

        public async Task<IEnumerable<Folder>> CopyAndPasteFoldersAsync(IEnumerable<Guid> folderIds, Guid targetFolderId)
        {
            var targetFolder = await _folderDatabaseService.GetFolderByIdAsync(targetFolderId);
            if(targetFolder == null)
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
        
        private async Task UploadFileRollbackAsync(UploadFileResult uploadFileResult, string absoluteFolderPath)
        {
            try
            {
                var absoluteFilePath = Path.Combine(absoluteFolderPath, uploadFileResult.Id.ToString());
                if (System.IO.File.Exists(absoluteFilePath))
                {
                    await _fileStorageService.DeleteFileAsync(absoluteFolderPath);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to rollback file", ex);
            }
        }
    }
}
