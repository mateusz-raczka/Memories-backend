using AutoMapper;
using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.File.Request;
using Memories_backend.Models.DTO.FileDetails.Request;

namespace Memories_backend.AutoMapper.Profiles
{
    public class FileMapperProfiles : Profile
    {
        public FileMapperProfiles()
        {
            CreateMap<Models.Domain.File, FileDtoFetchResponse>().ReverseMap();
            CreateMap<Models.Domain.File, FileDtoUpdateRequest>().ReverseMap();
            CreateMap<Models.Domain.File, FileDtoCreateRequest>().ReverseMap();
            CreateMap<Models.Domain.File, FileDtoCreateResponse>().ReverseMap();
            CreateMap<FileDtoCreateRequest, FileDtoCreateResponse>().ReverseMap();
            CreateMap<IFormFile, FileDtoCreateRequest>()
                .ForMember(dest => dest.FileDetails, opt => opt.MapFrom(src => new ComponentDetailsDtoRequest
                {
                    Name = src.FileName,
                    Size = src.Length,
                    CreatedDate = DateTime.UtcNow
                }));
        }
    }
}
