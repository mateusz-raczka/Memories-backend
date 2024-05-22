using AutoMapper;
using MemoriesBackend.API.DTO.FolderDetails.Request;
using MemoriesBackend.API.DTO.FolderDetails.Response;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FolderDetailsMapperProfiles : Profile
    {
        public FolderDetailsMapperProfiles()
        {
            CreateMap<FolderDetails, FolderDetailsRequest>().ReverseMap();
            CreateMap<FolderDetails, FolderDetailsResponse>().ReverseMap();
            CreateMap<FolderDetailsRequest, FolderDetailsResponse>().ReverseMap();
        }
    }
}
