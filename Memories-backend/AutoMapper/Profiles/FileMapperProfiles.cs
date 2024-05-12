using AutoMapper;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.FileDetails.Request;
using File = Memories_backend.Models.Domain.File;

namespace Memories_backend.AutoMapper.Profiles
{
    public class FileMapperProfiles : Profile
    {
        public FileMapperProfiles()
        {
            CreateMap<File, FileDtoFetchResponse>().ReverseMap();
            CreateMap<File, FileDtoUpdateRequest>().ReverseMap();
            CreateMap<File, FileDtoCreateRequest>().ReverseMap();
            CreateMap<File, FileDtoCreateResponse>().ReverseMap();
            CreateMap<FileDtoCreateRequest, FileDtoCreateResponse>().ReverseMap();
            CreateMap<IFormFile, FileDtoCreateRequest>()
                .ForMember(dest => dest.FileDetails, opt => opt.MapFrom(src => new FileDetailsDtoRequest
                {
                    Name = src.FileName,
                    Size = src.Length
                }));
        }
    }
}
