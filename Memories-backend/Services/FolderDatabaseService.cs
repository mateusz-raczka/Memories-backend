using AutoMapper;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Request;
using Memories_backend.Models.DTO.Folder.Response;
using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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

            folder.HierarchyId = await GenerateHierarchyId(null);

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

            Guid parentFolderId = createModel.FolderId;

            folder.HierarchyId = await GenerateHierarchyId(parentFolderId);

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

        public async Task<Folder> FindRootFolderAsync() =>
            await _folderRepository.GetRootFolderAsync();

        public async Task<string> GetFolderRelativePathAsync(Guid folderId)
        {
            List<Folder> folderHierarchy = await _folderRepository.GetFolderAncestorsAsync(folderId);

            IEnumerable<Guid> folderIds = folderHierarchy.Select(x => x.Id);

            string path = string.Join("/", folderIds);

            return path;
        }

        private async Task<HierarchyId> GenerateHierarchyId(Guid? parentFolderId)
        {
            if (parentFolderId == null)
            {
                return HierarchyId.GetRoot();
            }

            Folder parentFolder = await _folderRepository.GetById(parentFolderId.Value);

            HierarchyId parentHierarchyId = parentFolder.HierarchyId;

            HierarchyId childHierarchyId = parentHierarchyId.GetDescendant(null, null);

            return childHierarchyId;
        }
    }
}
