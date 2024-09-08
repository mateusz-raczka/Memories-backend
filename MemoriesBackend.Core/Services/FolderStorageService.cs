using MemoriesBackend.Domain.Interfaces.Services;

namespace MemoriesBackend.Application.Services
{
    public class FolderStorageService : IFolderStorageService
    {
        public async Task DeleteFolderAsync(string absoluteFolderPath)
        {
            if (!Directory.Exists(absoluteFolderPath))
            {
                throw new ApplicationException($"Failed to delete folder from file storage - it was not found");
            }

            await Task.Run(() => Directory.Delete(absoluteFolderPath, recursive: true));
        }
    }
}
