﻿using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.StaticFiles;
using System.IO.Compression;
using File = System.IO.File;

namespace MemoriesBackend.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        public FileStorageService() { }

        public async Task<FileUploadedMetaData> UploadFileAsync(IFormFile file, string absoluteFolderPath)
        {
            var uploadedFile = new FileUploadedMetaData
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

        public async Task<FileContentResult> DownloadFilesAsZipAsync(IEnumerable<string> absoluteFilePaths)
        {
            var files = new Dictionary<string, byte[]>();

            foreach(var filePath in absoluteFilePaths)
            {
                if (File.Exists(filePath))
                {
                    var fileBytes = await File.ReadAllBytesAsync(filePath);
                    var fileName = Path.GetFileName(filePath);
                    files.Add(fileName, fileBytes);
                }
            }

            if (files.Count == 0)
            {
                throw new ApplicationException("No files found to download.");
            }

            var zipBytes = CreateZipArchive(files);
            var zipFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.zip";
            var contentType = "application/zip";

            return new FileContentResult(zipBytes, contentType)
            {
                FileDownloadName = zipFileName
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

        public async Task MoveFileAsync(string fileAbsolutePath, string destinationFolderAbsolutePath)
        {
            if (string.IsNullOrWhiteSpace(fileAbsolutePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(fileAbsolutePath));

            if (string.IsNullOrWhiteSpace(destinationFolderAbsolutePath))
                throw new ArgumentException("Destination folder path cannot be null or empty", nameof(destinationFolderAbsolutePath));

            if (!File.Exists(fileAbsolutePath))
                throw new FileNotFoundException("Source file not found", fileAbsolutePath);

            if (!Directory.Exists(destinationFolderAbsolutePath))
                Directory.CreateDirectory(destinationFolderAbsolutePath);

            var fileName = Path.GetFileName(fileAbsolutePath);
            var newFileAbsolutePath = Path.Combine(destinationFolderAbsolutePath, fileName);

            await Task.Run(() => File.Move(fileAbsolutePath, newFileAbsolutePath, overwrite: true));
        }

        public async Task<FileStreamResult> StreamFileAsync(string absoluteFilePath)
        {
            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("The requested file does not exist.");

            var fileExtension = Path.GetExtension(absoluteFilePath);
            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(fileExtension, out var contentType))
                contentType = "application/octet-stream";

            var fileStream = new FileStream(absoluteFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 64 * 1024, useAsync: true);

            var result = new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = null,
                EnableRangeProcessing = contentType.StartsWith("video/") || contentType.StartsWith("audio/")
            };

            return result;
        }

        public async Task<Guid> UploadFileChunkAsync(IFormFile fileData, string absoluteFolderPath, Guid fileId)
        {
            Guid fileChunkId = Guid.NewGuid();
            var absoluteTempFolderPath = Path.Combine(absoluteFolderPath, $"Temp_{fileId}");
            var absoluteFileChunkPath = Path.Combine(absoluteTempFolderPath, fileChunkId.ToString());

            if (!Directory.Exists(absoluteTempFolderPath))
                Directory.CreateDirectory(absoluteTempFolderPath);

            using (var fileStream = new FileStream(absoluteFileChunkPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await fileData.CopyToAsync(fileStream);
            }

            return fileChunkId;
        }

        public async Task MergeFileChunksAsync(FileUploadProgress uploadProgress, string absoluteFolderPath)
        {
            var absoluteTempFolderPath = Path.Combine(absoluteFolderPath, $"Temp_{uploadProgress.Id}");
            var fileIdWithExtension = uploadProgress.Id.ToString() + uploadProgress.Extension;
            var finalAbsoluteFilePath = Path.Combine(absoluteFolderPath, fileIdWithExtension);

            using (var finalFileStream = new FileStream(finalAbsoluteFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var orderedChunks = uploadProgress.FileChunks.OrderBy(fc => fc.ChunkIndex).ToList();

                foreach (var chunk in orderedChunks)
                {
                    var absoluteFileChunkPath = Path.Combine(absoluteTempFolderPath, chunk.Id.ToString());

                    using (var chunkFileStream = new FileStream(absoluteFileChunkPath, FileMode.Open, FileAccess.Read))
                    {
                        await chunkFileStream.CopyToAsync(finalFileStream);
                    }
                }
            }
        }

        public async Task MergeAndDeleteFileChunksAsync(FileUploadProgress uploadProgress, string absoluteFolderPath)
        {
            await MergeFileChunksAsync(uploadProgress, absoluteFolderPath);

            var absoluteTempFolderPath = Path.Combine(absoluteFolderPath, $"Temp_{uploadProgress.Id}");

            DeleteTempFolder(absoluteTempFolderPath);
        }

        public async Task ChangeFileExtensionAsync(string absoluteFilePath, string newExtension)
        {
            if (string.IsNullOrWhiteSpace(absoluteFilePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(absoluteFilePath));

            if (!File.Exists(absoluteFilePath))
                throw new FileNotFoundException("File not found", absoluteFilePath);

            if (!newExtension.StartsWith("."))
                throw new ApplicationException("File extension must start with: '.'");

            var newFilePath = Path.ChangeExtension(absoluteFilePath, newExtension);

            await Task.Run(() => File.Move(absoluteFilePath, newFilePath, overwrite: true));
        }


        private void DeleteTempFolder(string absoluteTempFolderPath)
        {
            var tempFolderPath = Path.Combine(absoluteTempFolderPath);

            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
        }

        private byte[] CreateZipArchive(Dictionary<string, byte[]> files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = zipArchive.CreateEntry(file.Key, CompressionLevel.Fastest);
                        using (var entryStream = zipEntry.Open())
                        {
                            entryStream.Write(file.Value, 0, file.Value.Length);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
