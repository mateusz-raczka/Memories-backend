using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Application.Interfaces
{
    public interface IFileUploadProgressDatabaseService
    {
        Task<FileUploadProgress> GetFileUploadProgressByIdAsync(Guid fileUploadProgressId, bool asNoTracking = true);
        Task<FileUploadProgress> GetFileUploadProgressByIdWithRelationsAsync(Guid fileUploadProgressId, bool asNoTracking = true);
        Task<FileUploadProgress> CreateFileUploadProgressAsync(FileUploadProgress fileUploadProgress);
        Task DeleteFileUploadProgressAsync(Guid fileUploadProgressId);
        void DeleteFileUploadProgressAsync(FileUploadProgress fileUploadProgress);
        void UpdateFileUploadProgressAsync(FileUploadProgress fileUploadProgress);
        Task SaveAsync();
    }
}
