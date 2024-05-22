using AutoMapper;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.API.DTO.Tag.Request;
using MemoriesBackend.API.DTO.Tag.Response;

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
