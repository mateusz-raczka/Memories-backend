using AutoMapper;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.Tag.Request;
using Memories_backend.Models.DTO.FileDetails.Response;
using Memories_backend.Models.DTO.Tag.Response;
using Memories_backend.Models.DTO.FileDetails.Request;

namespace Memories_backend.AutoMapper.Profiles
{
    public class FileMapperProfiles : Profile
    {
        public FileMapperProfiles()
        {
            CreateMap<Models.Domain.Folder.File.File, FileDtoFetchResponse>().ReverseMap();
            CreateMap<Models.Domain.Folder.File.File, FileDtoUpdateRequest>().ReverseMap();
            CreateMap<Models.Domain.Folder.File.File, FileDtoCreateRequest>().ReverseMap();
            CreateMap<Models.Domain.Folder.File.File, FileDtoCreateResponse>().ReverseMap();
            CreateMap<FileDtoCreateRequest, FileDtoCreateResponse>().ReverseMap();
        }
    }
}
