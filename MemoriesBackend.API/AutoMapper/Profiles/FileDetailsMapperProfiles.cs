using AutoMapper;
using MemoriesBackend.API.DTO.FileDetails.Request;
using MemoriesBackend.API.DTO.FileDetails.Response;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FileDetailsMapperProfiles : Profile
    {
        public FileDetailsMapperProfiles()
        {
            CreateMap<FileDetails, FileDetailsResponse>().ReverseMap();
            CreateMap<FileDetails, FileDetailsUpdateRequest>().ReverseMap();
        }
    }
}
