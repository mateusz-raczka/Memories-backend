using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace MemoriesBackend.Application.Services
{
    public class PathService : IPathService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;

        public PathService(
            IConfiguration configuration,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService
        )
        {
            _configuration = configuration;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
        }

        public async Task<string> GetFileAbsolutePathAsync(Guid fileId)
        {
            var relativeFilePath = await GetFileRelativePathAsync(fileId);

            var absoluteFilePath = GetAbsolutePath(relativeFilePath);

            return absoluteFilePath;
        }

        public async Task<string> GetFileRelativePathAsync(Guid fileId)
        {
            var file = await _fileDatabaseService.GetFileByIdAsync(fileId);

            if (file == null) throw new ApplicationException("Cannot find file with a given id");
            var fileExtension = Path.GetExtension(file.FileDetails.Name);

            var fileIdWithExtension = fileId + fileExtension;

            var relativeFolderPath = await GetFolderRelativePathAsync(file.FolderId);

            var relativeFilePath = Path.Combine(relativeFolderPath, fileIdWithExtension);

            return relativeFilePath;
        }

        public async Task<string> GetFolderAbsolutePathAsync(Guid folderId)
        {
            var relativeFolderPath = await GetFolderRelativePathAsync(folderId);

            var absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            return absoluteFolderPath;
        }

        public async Task<string> GetFolderRelativePathAsync(Guid folderId)
        {
            var folderHierarchy = await _folderDatabaseService.GetFolderDescendantsAsync(folderId);

            var folderIds = folderHierarchy.Select(x => x.Id);

            var path = string.Join("/", folderIds);

            return path;
        }

        public string GetAbsolutePath(string path)
        {
            return Path.Combine(_configuration["Storage:Path"] ?? "", path);
        }
    }
}
