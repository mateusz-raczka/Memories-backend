using AutoMapper;
using MemoriesBackend.API.DTO.File.Request;
using MemoriesBackend.API.DTO.File.Response;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FileMapperProfiles : Profile
    {
        public FileMapperProfiles()
        {
            CreateMap<File, FileGetAllResponse>().ReverseMap();
            CreateMap<File, FileUpdateRequest>().ReverseMap();
            CreateMap<File, FileCreateRequest>().ReverseMap();
            CreateMap<File, FileCreateResponse>().ReverseMap();
            CreateMap<FileCreateRequest, FileCreateResponse>().ReverseMap();
            CreateMap<File, FileGetByIdResponse>().ReverseMap();
        }
    }
}
