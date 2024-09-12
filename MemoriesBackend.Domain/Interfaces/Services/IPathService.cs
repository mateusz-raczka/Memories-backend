namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IPathService
    {
        Task<string> GetFolderRelativePathAsync(Guid folderId);
        Task<string> GetFileRelativePathAsync(Guid fileId);
        Task<string> GetFileAbsolutePathAsync(Guid fileId);
        Task<string> GetFolderAbsolutePathAsync(Guid folderId);
        string GetAbsolutePath(string path);
    }
}
