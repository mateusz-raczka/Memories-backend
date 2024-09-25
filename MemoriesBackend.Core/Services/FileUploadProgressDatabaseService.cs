using MemoriesBackend.Application.Interfaces;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Application.Services
{
    public class FileUploadProgressDatabaseService : IFileUploadProgressDatabaseService
    {
        private readonly IFileUploadProgressRepository _fileUploadProgressRepository;

        public FileUploadProgressDatabaseService(
            IFileUploadProgressRepository fileUploadProgressRepository
            ) 
        {
            _fileUploadProgressRepository = fileUploadProgressRepository;
        }

        public async Task<FileUploadProgress> GetFileUploadProgressByIdAsync(Guid fileUploadProgressId, bool asNoTracking = true)
        {
            return await _fileUploadProgressRepository.GetById(fileUploadProgressId, asNoTracking);
        }

        public async Task<FileUploadProgress> GetFileUploadProgressByIdWithRelationsAsync(Guid fileUploadProgressId, bool asNoTracking = true)
        {
            return await _fileUploadProgressRepository.GetFileUploadProgressByIdWithRelationsAsync(fileUploadProgressId, asNoTracking);
        }

        public async Task<FileUploadProgress> CreateFileUploadProgressAsync(FileUploadProgress fileUploadProgress)
        {
            if(fileUploadProgress == null)
            {
                throw new ApplicationException("Failed to create - file upload progress is null");
            }

            return await _fileUploadProgressRepository.Create( fileUploadProgress );
        }

        public async Task DeleteFileUploadProgressAsync(Guid fileUploadProgressId)
        {
            var fileUploadProgress = await GetFileUploadProgressByIdWithRelationsAsync(fileUploadProgressId);

            if(fileUploadProgress == null)
            {
                throw new ApplicationException($"Failed to delete - file upload progress with Id {fileUploadProgressId} was not found");
            }

            _fileUploadProgressRepository.Delete(fileUploadProgress);
        }

        public void DeleteFileUploadProgressAsync(FileUploadProgress fileUploadProgress)
        {
            if (fileUploadProgress == null)
            {
                throw new ApplicationException($"Failed to delete - file upload progress is null");
            }

            _fileUploadProgressRepository.Delete(fileUploadProgress);
        }

        public void UpdateFileUploadProgressAsync(FileUploadProgress fileUploadProgress)
        {
            if(fileUploadProgress == null)
            {
                throw new ApplicationException("Failed to update - file upload progress is null");
            }

            _fileUploadProgressRepository.Update( fileUploadProgress );
        }

        public async Task SaveAsync()
        {
            await _fileUploadProgressRepository.Save();
        }
    }
}
