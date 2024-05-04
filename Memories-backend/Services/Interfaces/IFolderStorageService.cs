namespace Memories_backend.Services.Interfaces
{
    public interface IFolderStorageService
    {
        Task DeleteFolderAsync(Guid folderId);
    }
}
