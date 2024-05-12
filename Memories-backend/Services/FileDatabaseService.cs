using AutoMapper;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;
using System.Linq.Expressions;
using File = Memories_backend.Models.Domain.File;

namespace Memories_backend.Services
{
    public class FileDatabaseService : IFileDatabaseService
    {
        private readonly ISQLRepository<File> _fileRepository;
        private readonly IMapper _mapper;
        public FileDatabaseService(
            ISQLRepository<File> fileRepository,
            IMapper mapper
            )
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<FileDtoCreateResponse> CreateFileAsync(FileDtoCreateRequest createModel)
        {
            File fileDomain = _mapper.Map<File>(createModel);

            File createdFile = await _fileRepository.Create(fileDomain);

            FileDtoCreateResponse fileDto = _mapper.Map<FileDtoCreateResponse>(createdFile);

            await _fileRepository.Save();

            return fileDto;
        }


        public async Task UpdateFileAsync(Guid id, FileDtoUpdateRequest updateModel)
        {
            File file = await _fileRepository.GetById(id);

            if (file == null)
            {
                throw new KeyNotFoundException("Invalid FileId.");
            }

            _mapper.Map(updateModel, file);
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

        public async Task<FileDtoFetchResponse> GetFileByIdAsync(Guid id)
        {
            File file = await _fileRepository.GetById(id);

            if(file == null)
            {
                throw new KeyNotFoundException("Failed to fetch - There was no file found with given id.");
            }

            FileDtoFetchResponse fileDto = _mapper.Map<FileDtoFetchResponse>(file);

            return fileDto;
        }

        public async Task<IEnumerable<FileDtoFetchResponse>> GetAllFilesAsync(
            int? pageNumber,
            int? pageSize,
            Expression<Func<File, bool>>? filter = null,
            Func<IQueryable<File>, IOrderedQueryable<File>>? orderBy = null
            )
        {
            IEnumerable<File> files = await _fileRepository.GetAll(
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
