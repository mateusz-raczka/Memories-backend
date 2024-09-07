namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFolderStorageService
    {
        Task DeleteFolderAsync(string absoluteFolderPath);
    }
}
