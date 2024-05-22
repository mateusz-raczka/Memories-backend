namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IFolderStorageService
    {
        Task DeleteFolderAsync(Guid folderId);
    }
}
