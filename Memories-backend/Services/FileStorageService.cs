using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Memories_backend.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDatabaseService _fileService;
        private readonly IPathService _pathService;
        public FileStorageService(
            IConfiguration configuration,
            IFileDatabaseService fileService,
            IPathService pathService
            )
        {
            _configuration = configuration;
            _fileService = fileService;
            _pathService = pathService;
        }

        public async Task<Guid> UploadFileAsync(IFormFile file, Guid folderId)
        {
            Guid fileId = Guid.NewGuid();

            string folderPath = await _pathService.GetFolderPath(folderId);
            string fullDirectoryPath = CreateFullPath(folderPath);
            string fullFilePath = Path.Combine(fullDirectoryPath, fileId.ToString());

            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileId;
        }

        public async Task<FileContentResult> DownloadFileAsync(Guid id, Guid folderId)
        {
            // Do poprawek
            string folderPath = await _pathService.GetFolderPath(folderId);
            string fullDirectoryPath = CreateFullPath(folderPath);
            string fullFilePath = Path.Combine(fullDirectoryPath, id.ToString());

            if (!System.IO.File.Exists(fullFilePath))
            {
                throw new FileNotFoundException($"File '{id}' not found.");
            }

            byte[] fileContents = await System.IO.File.ReadAllBytesAsync(fullFilePath);

            FileDtoFetchResponse file = await _fileService.GetFileByIdAsync(id);

            string fileName = file.FileDetails.Name;

            var fileResult = new FileContentResult(fileContents, "application/octet-stream")
            {
                FileDownloadName = fileName
            };

            return fileResult;
        }

        public async Task DeleteFileAsync(Guid id, Guid folderId)
        {
            // Do poprawek
            string filePath = Path.Combine(CreateFullPath(await _pathService.GetFolderPath(folderId)), id.ToString());

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException($"File '{id}' not found.");
            }
        }

        private string CreateFullPath(string path)
        {
            return Path.Combine(_configuration["Storage:Path"], path);
        }
    }
}
