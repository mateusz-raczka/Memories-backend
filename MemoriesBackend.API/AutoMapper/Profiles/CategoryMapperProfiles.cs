using AutoMapper;
using MemoriesBackend.API.DTO.Category.Response;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class CategoryMapperProfiles : Profile
    {
        public CategoryMapperProfiles() 
        {
            CreateMap<Category, CategoryResponse>().ReverseMap();
        }
    }
}
