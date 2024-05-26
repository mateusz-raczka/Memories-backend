using MemoriesBackend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace MemoriesBackend.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDatabaseService _fileDatabaseService;
        private readonly IFolderDatabaseService _folderDatabaseService;

        public FileStorageService(
            IConfiguration configuration,
            IFileDatabaseService fileDatabaseService,
            IFolderDatabaseService folderDatabaseService
        )
        {
            _configuration = configuration;
            _fileDatabaseService = fileDatabaseService;
            _folderDatabaseService = folderDatabaseService;
        }

        public async Task<Guid> UploadFileAsync(IFormFile file, Guid folderId)
        {
            var fileId = Guid.NewGuid();

            var fileExtension = Path.GetExtension(file.FileName);

            var fileIdWithExtension = fileId + fileExtension;

            var relativeFolderPath = await _folderDatabaseService.GetFolderRelativePathAsync(folderId);

            var absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            try
            {
                if (!Directory.Exists(absoluteFolderPath))
                    Directory.CreateDirectory(absoluteFolderPath);

                using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileId;
            }
            catch (Exception ex)
            {
                if (File.Exists(absoluteFilePath))
                    File.Delete(absoluteFilePath);

                throw;
            }
        }

        public async Task<FileContentResult> DownloadFileAsync(Guid id)
        {
            var file = await _fileDatabaseService.GetFileByIdAsync(id);

            if (file == null) throw new ApplicationException("Cannot find file with a given id");
            var fileExtension = Path.GetExtension(file.FileDetails.Name);

            var fileIdWithExtension = id + fileExtension;

            var relativeFolderPath = await _folderDatabaseService.GetFolderRelativePathAsync(file.FolderId);

            var absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");

            var fileBytes = await File.ReadAllBytesAsync(absoluteFilePath);

            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(file.FileDetails.Name, out var contentType))
                contentType = "application/octet-stream";

            return new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = file.FileDetails.Name
            };
        }

        public async Task DeleteFileAsync(Guid id)
        {
            var file = await _fileDatabaseService.GetFileByIdAsync(id);
            if (file == null) throw new ApplicationException("Cannot find file with a given id.");

            var fileExtension = Path.GetExtension(file.FileDetails.Name);

            var fileIdWithExtension = id + fileExtension;

            var relativeFolderPath = await _folderDatabaseService.GetFolderRelativePathAsync(file.FolderId);

            var absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");

            File.Delete(absoluteFilePath);
        }

        private string GetAbsolutePath(string path)
        {
            return Path.Combine(_configuration["Storage:Path"], path);
        }
    }
}