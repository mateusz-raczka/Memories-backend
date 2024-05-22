using AutoMapper;
using MemoriesBackend.API.DTO.User;
using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class UserMapperProfiles : Profile
    {
        public UserMapperProfiles() 
        {
            CreateMap<UserData, UserDataResponse>().ReverseMap();
        }
    }
}
