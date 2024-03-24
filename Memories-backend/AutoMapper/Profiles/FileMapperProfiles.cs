using AutoMapper;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Category.Response;
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
            CreateMap<Models.Domain.File, FileDtoFetchResponse>().ReverseMap();

            CreateMap<Models.Domain.File, FileDtoUpdateRequest>().ReverseMap();

            CreateMap<Models.Domain.File, FileDtoCreateRequest>().ReverseMap();

            CreateMap<Models.Domain.File, FileDtoCreateResponse>().ReverseMap();

            CreateMap<FileDtoCreateRequest, FileDtoCreateResponse>().ReverseMap();

            CreateMap<Category, CategoryDtoResponse>().ReverseMap();

            CreateMap<Tag, TagDtoResponse>().ReverseMap();

            CreateMap<Tag, TagDtoRequest>().ReverseMap();

            CreateMap<FileDetails, FileDetailsDtoResponse>().ReverseMap();

            CreateMap<FileDetails, FileDetailsDtoRequest>().ReverseMap();
        }
    }
}
