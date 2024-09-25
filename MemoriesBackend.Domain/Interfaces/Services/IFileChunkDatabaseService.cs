using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IFileChunkDatabaseService
    {
        Task<FileChunk> CreateFileChunkAsync(FileChunk fileChunk);
        Task SaveAsync();
    }
}
