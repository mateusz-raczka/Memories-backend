using AutoMapper;
using Memories_backend.Models.Domain.Folder.File;
using Memories_backend.Models.DTO.FileDetails.Request;
using Memories_backend.Models.DTO.FileDetails.Response;

namespace Memories_backend.AutoMapper.Profiles
{
    public class FileDetailsMapperProfiles : Profile
    {
        public FileDetailsMapperProfiles() 
        {
            CreateMap<ComponentDetails, ComponentDetailsDtoResponse>().ReverseMap();
            CreateMap<ComponentDetails, ComponentDetailsDtoRequest>().ReverseMap();
        }
    }
}
