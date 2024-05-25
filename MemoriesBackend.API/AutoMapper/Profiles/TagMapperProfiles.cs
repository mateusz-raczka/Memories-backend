using AutoMapper;
using MemoriesBackend.API.DTO.Tag.Request;
using MemoriesBackend.API.DTO.Tag.Response;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class TagMapperProfiles : Profile
    {
        public TagMapperProfiles()
        {
            CreateMap<Tag, TagResponse>().ReverseMap();
            CreateMap<Tag, TagRequest>().ReverseMap();
        }
    }
}
