﻿using MemoriesBackend.Domain.Interfaces.Services;

namespace MemoriesBackend.Application.Services
{
    public class FolderStorageService : IFolderStorageService
    {
        public async Task DeleteFolderAsync(string absoluteFolderPath)
        {
            if (!Directory.Exists(absoluteFolderPath))
            {
                return;
            }

            await Task.Run(() => Directory.Delete(absoluteFolderPath, recursive: true));
        }

        public async Task MoveFolderAsync(string sourceFolderAbsolutePath, string destinationFolderAbsolutePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFolderAbsolutePath))
                throw new ArgumentException("Source folder path cannot be null or empty", nameof(sourceFolderAbsolutePath));

            if (string.IsNullOrWhiteSpace(destinationFolderAbsolutePath))
                throw new ArgumentException("Destination folder path cannot be null or empty", nameof(destinationFolderAbsolutePath));

            if (!Directory.Exists(sourceFolderAbsolutePath))
                return;

            if (!Directory.Exists(destinationFolderAbsolutePath))
                Directory.CreateDirectory(destinationFolderAbsolutePath);

            var sourceFolderName = Path.GetFileName(sourceFolderAbsolutePath.TrimEnd(Path.DirectorySeparatorChar));

            var newSourceFolderAbsolutePath = Path.Combine(destinationFolderAbsolutePath, sourceFolderName);

            await Task.Run(() => Directory.Move(sourceFolderAbsolutePath, newSourceFolderAbsolutePath));
        }
    }
}
