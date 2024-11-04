using AutoMapper;
using MemoriesBackend.API.DTO.User;
using MemoriesBackend.Domain.Models;

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
