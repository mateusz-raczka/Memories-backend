using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;

namespace MemoriesBackend.Application.Services
{
    public class FileChunkDatabaseService : IFileChunkDatabaseService
    {
        private readonly IGenericRepository<FileChunk> _fileChunkRepository;
        public FileChunkDatabaseService(
            IGenericRepository<FileChunk> fileChunkRepository
            )
        {
            _fileChunkRepository = fileChunkRepository;
        }

        public async Task<FileChunk> CreateFileChunkAsync(FileChunk fileChunk)
        {
            if(fileChunk == null)
            {
                throw new ApplicationException("Failed to create - file chunk is null");
            }

            return await _fileChunkRepository.Create(fileChunk);
        }

        public async Task SaveAsync()
        {
            await _fileChunkRepository.Save();
        }
    }
}
