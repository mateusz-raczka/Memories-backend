namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderStorageService
    {
        Task DeleteFolderAsync(string absoluteFolderPath);
        Task MoveFolderAsync(string sourceFolderAbsolutePath, string destinationFolderAbsolutePath);
    }
}
