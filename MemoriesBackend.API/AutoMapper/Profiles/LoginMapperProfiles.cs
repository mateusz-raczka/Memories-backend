using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Response;
using MemoriesBackend.Domain.Models.Authentication;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class LoginMapperProfiles : Profile
    {
        public LoginMapperProfiles()
        {
            CreateMap<Login, LoginResponse>().ReverseMap();
        }
    }
}
