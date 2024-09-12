using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MemoriesBackend.Domain.Models.Storage;
using MemoriesBackend.Domain.Models.FileStorage;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Entities;
using File = System.IO.File;

namespace MemoriesBackend.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        IGenericRepository<FileUploadProgress> _fileUploadProgressRepository;

        public FileStorageService(IGenericRepository<FileUploadProgress> fileUploadProgressRepository)
        {
            _fileUploadProgressRepository = fileUploadProgressRepository;
        }

        public async Task<FileUploadedResult> UploadFileAsync(IFormFile file, string absoluteFolderPath)
        {
            var uploadedFile = new FileUploadedResult
            {
                Id = Guid.NewGuid(),
                Size = file.Length,
                Extension = Path.GetExtension(file.FileName),
                Name = file.FileName,
            };
            var fileIdWithExtension = uploadedFile.Id + uploadedFile.Extension;
            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

                if (!Directory.Exists(absoluteFolderPath))
                    Directory.CreateDirectory(absoluteFolderPath);

                using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return uploadedFile;
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

        public void DeleteFile(string absoluteFilePath)
        {
            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("Cannot find file with a given id in file storage.");

            File.Delete(absoluteFilePath);
        }

        public async Task<Guid> CopyAndPasteFileAsync(string fileAbsolutePath, string destinationFolderAbsolutePath)
        {
            if (string.IsNullOrWhiteSpace(fileAbsolutePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(fileAbsolutePath));

            if (string.IsNullOrWhiteSpace(destinationFolderAbsolutePath))
                throw new ArgumentException("Destination folder path cannot be null or empty", nameof(destinationFolderAbsolutePath));

            if (!File.Exists(fileAbsolutePath))
                throw new FileNotFoundException("Source file not found", fileAbsolutePath);

            if (!Directory.Exists(destinationFolderAbsolutePath))
                Directory.CreateDirectory(destinationFolderAbsolutePath);

            var fileId = Guid.NewGuid();
            var extension = Path.GetExtension(fileAbsolutePath);
            var newFileName = $"{fileId}{extension}";
            var destinationFilePath = Path.Combine(destinationFolderAbsolutePath, newFileName);

            await Task.Run(() => File.Copy(fileAbsolutePath, destinationFilePath, overwrite: true));

            return fileId;
        }

        public FileStreamResult StreamFile(string absoluteFilePath)
        {
            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("The requested file does not exist.");

            var fileExtension = Path.GetExtension(absoluteFilePath);
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (!contentTypeProvider.TryGetContentType(fileExtension, out var contentType))
                contentType = "application/octet-stream";

            var fileStream = new FileStream(absoluteFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var result = new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = null
            };

            if (contentType.StartsWith("video/") || contentType.StartsWith("audio/"))
            {
                result.EnableRangeProcessing = true;
            }

            return result;
        }

        public async Task UploadFileChunkAsync(Stream stream, FileChunkMetaData fileChunkMetaData, string absoluteFolderPath)
        {
            var fileExtension = Path.GetExtension(fileChunkMetaData.FileName);
            var fileIdWithExtension = fileChunkMetaData.Id + fileExtension;
            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            using (var fileStream = new FileStream(absoluteFilePath, FileMode.Append))
            {
                await stream.CopyToAsync(stream);
            }
        }
    }
}
