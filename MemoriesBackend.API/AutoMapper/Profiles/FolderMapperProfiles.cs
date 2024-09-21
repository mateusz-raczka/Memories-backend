using AutoMapper;
using MemoriesBackend.API.DTO.Folder.Request;
using MemoriesBackend.API.DTO.Folder.Response;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FolderMapperProfiles : Profile
    {
        public FolderMapperProfiles()
        {
            CreateMap<Folder, FolderAddResponse>().ReverseMap();
            CreateMap<Folder, FolderAddRequest>().ReverseMap();
            CreateMap<Folder, FolderGetAllResponse>().ReverseMap();
            CreateMap<Folder, FolderGetByIdResponse>().ReverseMap();
            CreateMap<Folder, FolderCopyAndPasteResponse>().ReverseMap();
            CreateMap<Folder, FolderPathSegmentResposne>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FolderDetails.Name))
                .ReverseMap();
            CreateMap<FolderWithDescendants, FolderGetByIdWithPathResponse>()
                .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Descendants))
                .ReverseMap();
            CreateMap<Folder, FolderCutAndPasteResponse>().ReverseMap();
        }
    }
}
