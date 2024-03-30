namespace Memories_backend.Services
{
    public interface IFileSystemService
    {
        Task UploadFileAsync(byte[] file, Guid id);
        Task<byte[]> DownloadFileAsync(Guid id);
        void DeleteFile(Guid id);
    }
}
