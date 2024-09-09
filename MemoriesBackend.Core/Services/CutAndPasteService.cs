using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;

namespace MemoriesBackend.Application.Services
{
    public class CutAndPasteService
    {
        IFileDatabaseService _fileDatabaseService;
        IFileStorageService _fileStorageService;
        IFolderDatabaseService _folderDatabaseService;
        IPathService _pathService;
        ITransactionHandler _transactionHandler;

        public CutAndPasteService(
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

    }
}
