using AutoMapper;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Repositories;
using System.Linq.Expressions;

namespace Memories_backend.Services
{
    public class FileService : IFileService
    {
        private readonly ISQLRepository<Models.Domain.Folder.File.File> _fileRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        public FileService(
            ISQLRepository<Models.Domain.Folder.File.File> fileRepository,
            IFileStorageService fileStorageService,
            IMapper mapper
            )
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<FileDtoCreateResponse> CreateFileAsync(FileDtoCreateRequest requestBody)
        {
            Models.Domain.Folder.File.File fileDomain = _mapper.Map<Models.Domain.Folder.File.File>(requestBody);

            Models.Domain.Folder.File.File createdFile = await _fileRepository.Create(fileDomain);

            FileDtoCreateResponse fileDto = _mapper.Map<FileDtoCreateResponse>(createdFile);

            await _fileRepository.Save();

            return fileDto;
        }


        public async Task UpdateFileAsync(Guid id, FileDtoUpdateRequest requestBody)
        {
            Models.Domain.Folder.File.File file = await _fileRepository.GetById(id);

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
            _fileStorageService.DeleteFile(id);

            await _fileRepository.Delete(id);
            await _fileRepository.Save();
        }

        public async Task DeleteFileAsync(Models.Domain.Folder.File.File file)
        {
            _fileStorageService.DeleteFile(file.Id);

            _fileRepository.Delete(file);
            await _fileRepository.Save();
        }

        public async Task<FileDtoFetchResponse> GetFileByIdAsync(Guid id)
        {
            Models.Domain.Folder.File.File file = await _fileRepository.GetById(id);

            if(file == null)
            {
                throw new KeyNotFoundException("Failed to fetch - There was no file found with given id");
            }

            FileDtoFetchResponse fileDto = _mapper.Map<FileDtoFetchResponse>(file);

            return fileDto;
        }

        public async Task<IEnumerable<FileDtoFetchResponse>> GetAllFiles(
            int pageNumber,
            int pageSize,
            Expression<Func<Models.Domain.Folder.File.File, bool>> filter = null,
            Func<IQueryable<Models.Domain.Folder.File.File>, IOrderedQueryable<Models.Domain.Folder.File.File>> orderBy = null
            )
        {
            IEnumerable<Models.Domain.Folder.File.File> files = await _fileRepository.GetAll(
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
