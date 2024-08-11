using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace MemoriesBackend.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IPathService _pathService;

        public FileStorageService(IPathService pathService)
        {
            _pathService = pathService;
        }

        public async Task<Guid> UploadFileAsync(IFormFile file, string absoluteFolderPath)
        {
            var fileId = Guid.NewGuid();

            var fileExtension = Path.GetExtension(file.FileName);

            var fileIdWithExtension = fileId + fileExtension;

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

        public async Task<FileContentResult> DownloadFileAsync(string absoluteFilePath)
        {
            var fileName = Path.GetFileName(absoluteFilePath);

            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");

            var fileBytes = await File.ReadAllBytesAsync(absoluteFilePath);

            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(fileName, out var contentType))
                contentType = "application/octet-stream";

            return new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName
            };
        }

        public async Task DeleteFileAsync(string absoluteFilePath)
        {
            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");

            File.Delete(absoluteFilePath);
        }
    }
}