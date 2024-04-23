using AutoMapper;
using Memories_backend.Models.DTO.Folder.Response;
using Memories_backend.Models.DTO.Folder.Request;
using Memories_backend.Models.Domain;

namespace Memories_backend.AutoMapper.Profiles
{
    public class FolderMapperProfiles : Profile
    {
        public FolderMapperProfiles()
        {
            CreateMap<Folder, FolderDtoCreateResponse>().ReverseMap();
            CreateMap<Folder, FolderDtoCreateRequest>().ReverseMap();
            CreateMap<Folder, FolderDtoFetchResponse>().ReverseMap();
        }
    }
}
