using AutoMapper;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Request;
using Memories_backend.Models.DTO.Folder.Response;
using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;
using System.Linq.Expressions;
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
            Folder folder = new Folder();

            Folder createdFolder = await _folderRepository.Create(folder);

            FolderDtoCreateResponse folderDto = _mapper.Map<FolderDtoCreateResponse>(createdFolder);

            await _folderRepository.Save();

            return folderDto;
        }

        public async Task<IEnumerable<FolderDtoFetchAllResponse>> GetAllFoldersAsync(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<Folder, bool>>? filter = null,
            Func<IQueryable<Folder>, IOrderedQueryable<Folder>>? orderBy = null
            )
        {
            IEnumerable<Folder> folders = await _folderRepository.GetAll(
                pageNumber,
                pageSize,
                filter,
                orderBy
                );

            IEnumerable<FolderDtoFetchAllResponse> foldersDto = _mapper.Map<IEnumerable<FolderDtoFetchAllResponse>>(folders);

            return foldersDto;
        }



        public async Task<FolderDtoFetchByIdResponse> GetFolderByIdAsync(Guid folderId)
        {
            Folder folder = await _folderRepository.GetById(folderId);

            if (folder == null)
            {
                throw new KeyNotFoundException("Failed to fetch - There was no folder found with given id.");
            }

            FolderDtoFetchByIdResponse folderDto = _mapper.Map<FolderDtoFetchByIdResponse>(folder);

            folderDto.ChildFolders = await GetSubFoldersAsync(folderId);

            return folderDto;
        }

        public async Task<FolderDtoCreateResponse> CreateFolderAsync(FolderDtoCreateRequest createModel)
        {
            Folder folder = _mapper.Map<Folder>(createModel);

            Folder createdFolder = await _folderRepository.Create(folder);

            FolderDtoCreateResponse folderDto = _mapper.Map<FolderDtoCreateResponse>(createdFolder);

            await _folderRepository.Save();

            return folderDto;
        }

        public async Task<IEnumerable<FolderDtoFetchAllResponse>> GetSubFoldersAsync(Guid folderId)
        {
            Expression<Func<Folder, bool>> filter = entity => entity.FolderId == folderId;

            return await GetAllFoldersAsync(filter: filter);
        }

        public async Task<bool> FolderExistsAsync(Guid folderId)
        {
            Folder folder = await _folderRepository.GetById(folderId);

            if (folder == null)
            {
                return false;
            }

            return true;
        }

        public async Task<Folder> FindRootFolderAsync() =>
            await _folderRepository.FindRootFolderAsync();

        public async Task<IEnumerable<Guid>> GetFolderAncestorsIdsAsync(Guid folderId)
        {
            var folderHierarchy = await _folderRepository.GetFolderHierarchyAsync(folderId);

            if (folderHierarchy == null)
            {
                throw new KeyNotFoundException("Folder with given id does not exist");
            }

            var folderIds = folderHierarchy.Select(f => f.Id);

            return folderIds;
        }

        public async Task<string> GetFolderRelativePathAsync(Guid folderId)
        {
            IEnumerable<Guid> folderHierarchy = await GetFolderAncestorsIdsAsync(folderId);

            string path = string.Join("/", folderHierarchy);

            return path;
        }
    }
}
