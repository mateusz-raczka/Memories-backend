using AutoMapper;
using MemoriesBackend.Domain.Models;
using LoginRequest = MemoriesBackend.API.DTO.Authentication.Request.LoginRequest;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class LoginMapperProfiles : Profile
    {
        public LoginMapperProfiles()
        {
            CreateMap<Login, LoginRequest>().ReverseMap();
        }
    }
}
