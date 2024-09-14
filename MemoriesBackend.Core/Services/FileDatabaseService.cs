using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    public class FileDatabaseService : IFileDatabaseService
    {
        private readonly IGenericRepository<File> _fileRepository;

        public FileDatabaseService(IGenericRepository<File> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<File> CreateFileAsync(File file)
        {
            var createdFile = await _fileRepository.Create(file);
            await _fileRepository.Save();
            return createdFile;
        }

        public async Task UpdateFileAsync(Guid id, File file)
        {
            if (file == null) throw new ApplicationException("Invalid FileId.");
            _fileRepository.Update(file);
            await _fileRepository.Save();
        }

        public async Task DeleteFileAsync(Guid id)
        {
            await _fileRepository.Delete(id);
            await _fileRepository.Save();
        }

        public async Task DeleteFileAsync(File file)
        {
            _fileRepository.Delete(file);
            await _fileRepository.Save();
        }

        public async Task<File> GetFileByIdAsync(Guid id)
        {
            var file = await _fileRepository.GetById(id);
            if (file == null) throw new ApplicationException("Failed to fetch - There was no file found with the given id.");
            return file;
        }

        public async Task<IEnumerable<File>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<File, bool>>? filter = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null,
            bool asNoTracking = true
        )
        {
            var files = await _fileRepository.GetAll(
                filter: filter,
                orderBy: orderBy,
                pageNumber: pageNumber,
                pageSize: pageSize,
                asNoTracking: asNoTracking
            ).ToListAsync();

            return files;
        }

        public async Task BatchCreateFilesAsync(IEnumerable<File> files)
        {
            foreach (var file in files)
            {
                await _fileRepository.Create(file);
            }
            await _fileRepository.Save();
        }
    }
}
