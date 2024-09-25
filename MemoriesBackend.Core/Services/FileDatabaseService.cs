using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    public class FileDatabaseService : IFileDatabaseService
    {
        private readonly IFileRepository _fileRepository;

        public FileDatabaseService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<File> CreateFileAsync(File file)
        {
            if(file == null)
            {
                throw new ApplicationException("Failed to create - file is null");
            }

            var createdFile = await _fileRepository.Create(file);
            return createdFile;
        }

        public async Task DeleteFileAsync(Guid fileId)
        {
            var file = await _fileRepository.GetById(fileId);
            if(file == null)
            {
                throw new ApplicationException($"Failed to delete - cannot find file with id {fileId}");
            }

            _fileRepository.Delete(file);
        }

        public void DeleteFileAsync(File file)
        {
            if (file == null)
            {
                throw new ApplicationException($"Failed to delete - file is null");
            }

            _fileRepository.Delete(file);
        }

        public async Task<File> GetFileByIdAsync(Guid fileId, bool asNoTracking = true)
        {
            var file = await _fileRepository.GetById(fileId, asNoTracking);
            if (file == null) throw new ApplicationException("Failed to fetch - There was no file found with the given id.");
            return file;
        }

        public async Task<File> GetFileByIdWithDetailsAsync(Guid fileId, bool asNoTracking = true)
        {
            return await _fileRepository.GetFileByIdWithDetailsAsync(fileId, asNoTracking);
        }

        public async Task<List<File>> GetFilesByIdsWithDetailsAsync(IEnumerable<Guid> fileIds, bool asNoTracking = true)
        {
            return await _fileRepository.GetFilesByIdsWithDetailsAsync(fileIds, asNoTracking);
        }

        public async Task<IEnumerable<File>> GetAllFilesAsync(
            Expression<Func<File, bool>> filter,
            int? pageNumber,
            int? pageSize,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null,
            bool asNoTracking = true
        )
        {
            var files = await _fileRepository
                .GetAll(filter, orderBy, pageNumber, pageSize, asNoTracking)
                .ToListAsync();

            return files;
        }

        public async Task BatchCreateFilesAsync(IEnumerable<File> files)
        {
            foreach (var file in files)
            {
                await CreateFileAsync(file);
            }
        }

        public void UpdateFile(File file)
        {
            if(file == null)
            {
                throw new ApplicationException("Failed to update - file is null");
            }

            _fileRepository.Update(file);
        }

        public async Task SaveAsync()
        {
            await _fileRepository.Save();
        }
    }
}
