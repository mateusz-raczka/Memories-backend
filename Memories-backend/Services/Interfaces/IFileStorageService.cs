namespace Memories_backend.Services
{
    public interface IFileStorageService
    {
        Task<Guid> UploadFileAsync(IFormFile file, string folderHierarchy);
        Task<byte[]> DownloadFileAsync(Guid id, string folderHierarchy);
        void DeleteFile(Guid id, string folderHierarchy);
    }
}
