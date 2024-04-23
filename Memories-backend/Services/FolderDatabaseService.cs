using AutoMapper;
using Memories_backend.Models.Authorization;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Response;
using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;

namespace Memories_backend.Services
{
    public class FolderDatabaseService : IFolderDatabaseService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IMapper _mapper;

        public FolderDatabaseService(
            IFolderRepository folderRepository,
            IMapper mapper
            ) 
        { 
            _folderRepository = folderRepository;
            _mapper = mapper;
        }

        public async Task<FolderDtoCreateResponse> CreateRootFolderAsync()
        {
            Folder folderDomain = new Folder();

            Folder createdFolder = await _folderRepository.Create(folderDomain);
            
            FolderDtoCreateResponse folderDto = _mapper.Map<FolderDtoCreateResponse>(createdFolder);
            
            await _folderRepository.Save();

            return folderDto;
        }

        public async Task<IEnumerable<FolderDtoFetchResponse>> GetAllFoldersAsync()
        {
            IEnumerable<Folder> folders = await _folderRepository.GetAll();

            IEnumerable<FolderDtoFetchResponse> foldersDto = _mapper.Map<IEnumerable<FolderDtoFetchResponse>>(folders);

            return foldersDto;
        }

        public async Task<Folder> FindRootFolderAsync() =>
            await _folderRepository.FindRootFolderAsync();
    }
}
