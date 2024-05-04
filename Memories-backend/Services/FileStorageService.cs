using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Memories_backend.Services
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
            Guid fileId = Guid.NewGuid();

            string fileExtension = Path.GetExtension(file.FileName);

            string fileIdWithExtension = fileId.ToString() + fileExtension;

            string relativeFolderPath = await _folderDatabaseService.GetFolderRelativePathAsync(folderId);

            string absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            string absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);
            }

            using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileId;
        }

        public async Task<FileContentResult> DownloadFileAsync(Guid id)
        {
            FileDtoFetchResponse file = await _fileDatabaseService.GetFileByIdAsync(id);

            if (file == null)
            {
                throw new ApplicationException("Cannot find file with a given id");
            }
            string fileExtension = Path.GetExtension(file.FileDetails.Name);

            string fileIdWithExtension = id.ToString() + fileExtension;

            string relativeFolderPath = await _folderDatabaseService.GetFolderRelativePathAsync(file.FolderId);

            string absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            string absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            if (!File.Exists(absoluteFilePath))
            {
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");
            }

            byte[] fileBytes = await File.ReadAllBytesAsync(absoluteFilePath);

            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(file.FileDetails.Name, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = file.FileDetails.Name
            };
        }
        public async Task DeleteFileAsync(Guid id)
        {
            FileDtoFetchResponse file = await _fileDatabaseService.GetFileByIdAsync(id);
            if (file == null)
            {
                throw new ApplicationException("Cannot find file with a given id.");
            }

            string fileExtension = Path.GetExtension(file.FileDetails.Name);

            string fileIdWithExtension = id.ToString() + fileExtension;

            string relativeFolderPath = await _folderDatabaseService.GetFolderRelativePathAsync(file.FolderId);

            string absoluteFolderPath = GetAbsolutePath(relativeFolderPath);

            string absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            if (!File.Exists(absoluteFilePath))
            {
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");
            }

            File.Delete(absoluteFilePath);
        }

        private string GetAbsolutePath(string path)
        {
            return Path.Combine(_configuration["Storage:Path"], path);
        }
    }
}
