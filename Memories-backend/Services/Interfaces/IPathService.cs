namespace Memories_backend.Services.Interfaces
{
    public interface IPathService
    {
        Task<string> GetFolderPath(Guid folderId);
    }
}
