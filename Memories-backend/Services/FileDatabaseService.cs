using AutoMapper;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;
using System.Linq.Expressions;

namespace Memories_backend.Services
{
    public class FileDatabaseService : IFileDatabaseService
    {
        private readonly ISQLRepository<Models.Domain.File> _fileRepository;
        private readonly IMapper _mapper;
        public FileDatabaseService(
            ISQLRepository<Models.Domain.File> fileRepository,
            IMapper mapper
            )
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<FileDtoCreateResponse> CreateFileAsync(FileDtoCreateRequest requestBody)
        {
            Models.Domain.File fileDomain = _mapper.Map<Models.Domain.File>(requestBody);

            Models.Domain.File createdFile = await _fileRepository.Create(fileDomain);

            FileDtoCreateResponse fileDto = _mapper.Map<FileDtoCreateResponse>(createdFile);

            await _fileRepository.Save();

            return fileDto;
        }


        public async Task UpdateFileAsync(Guid id, FileDtoUpdateRequest requestBody)
        {
            Models.Domain.File file = await _fileRepository.GetById(id);

            if (file == null)
            {
                throw new KeyNotFoundException("Invalid FileId.");
            }

            _mapper.Map(requestBody, file);
            _fileRepository.Update(file);
            await _fileRepository.Save();
        }

        public async Task DeleteFileAsync(Guid id)
        {
            await _fileRepository.Delete(id);
            await _fileRepository.Save();
        }

        public async Task DeleteFileAsync(Models.Domain.File file)
        {
            _fileRepository.Delete(file);
            await _fileRepository.Save();
        }

        public async Task<FileDtoFetchResponse> GetFileByIdAsync(Guid id)
        {
            Models.Domain.File file = await _fileRepository.GetById(id);

            if(file == null)
            {
                throw new KeyNotFoundException("Failed to fetch - There was no file found with given id.");
            }

            FileDtoFetchResponse fileDto = _mapper.Map<FileDtoFetchResponse>(file);

            return fileDto;
        }

        public async Task<IEnumerable<FileDtoFetchResponse>> GetAllFiles(
            int pageNumber,
            int pageSize,
            Expression<Func<Models.Domain.File, bool>> filter = null,
            Func<IQueryable<Models.Domain.File>, IOrderedQueryable<Models.Domain.File>> orderBy = null
            )
        {
            IEnumerable<Models.Domain.File> files = await _fileRepository.GetAll(
                pageNumber,
                pageSize,
                filter,
                orderBy
                );
            IEnumerable<FileDtoFetchResponse> filesDto = _mapper.Map<IEnumerable<FileDtoFetchResponse>>(files);

            return filesDto;
        }
    }
}
