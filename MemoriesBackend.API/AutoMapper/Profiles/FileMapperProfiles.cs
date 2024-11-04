using AutoMapper;
using MemoriesBackend.API.DTO.File.Response;
using MemoriesBackend.API.DTO.FileDetails.Request;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FileMapperProfiles : Profile
    {
        public FileMapperProfiles()
        {
            CreateMap<File, FileGetAllResponse>().ReverseMap();
            CreateMap<File, FileAddResponse>().ReverseMap();
            CreateMap<File, FileGetByIdResponse>().ReverseMap();
            CreateMap<File, FileCopyPasteResponse>().ReverseMap();
            CreateMap<File, FileCutAndPasteResponse>().ReverseMap();
        }
    }
}
