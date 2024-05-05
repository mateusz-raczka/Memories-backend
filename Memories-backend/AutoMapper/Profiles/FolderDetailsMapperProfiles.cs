using AutoMapper;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.FolderDetails.Request;
using Memories_backend.Models.DTO.FolderDetails.Response;

namespace Memories_backend.AutoMapper.Profiles
{
    public class FolderDetailsMapperProfiles : Profile
    {
        public FolderDetailsMapperProfiles()
        {
            CreateMap<FolderDetails, FolderDetailsDtoRequest>().ReverseMap();
            CreateMap<FolderDetails, FolderDetailsDtoResponse>().ReverseMap();
            CreateMap<FolderDetailsDtoRequest, FolderDetailsDtoResponse>().ReverseMap();
        }
    }
}
