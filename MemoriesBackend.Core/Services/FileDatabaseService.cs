using System.Linq.Expressions;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Services
{
    public class FileDatabaseService : IFileDatabaseService
    {
        private readonly IGenericRepository<File> _fileRepository;

        public FileDatabaseService(
            IGenericRepository<File> fileRepository
        )
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
            if (file == null) throw new KeyNotFoundException("Invalid FileId.");

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

            if (file == null) throw new KeyNotFoundException("Failed to fetch - There was no file found with given id.");

            return file;
        }

        public async Task<IEnumerable<File>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<File, bool>>? filter = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null
        )
        {
            var files = await _fileRepository.GetAll(
                pageNumber,
                pageSize,
                filter,
                orderBy
            );

            return files;
        }

        public async Task<File> CopyFileAsync(Guid fileId)
        {
            var fileCopy = await _fileRepository
                    .GetQueryable()
                    .Include(f => f.FileActivities)
                    .Include(f => f.FileDetails)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == fileId);

            fileCopy.Id = Guid.NewGuid();
            fileCopy.FileActivities = new List<FileActivity>();
            fileCopy.FileDetails.Name = fileCopy.FileDetails.Name + $"COPY test";
            fileCopy.FileDetails.Id = Guid.NewGuid();

            if (fileCopy == null)
            {
                throw new ApplicationException($"File with Id {fileId} not found.");
            }

            return fileCopy;
        }
    }
}