using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Response;
using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class AuthMapperProfiles : Profile
    {
        public AuthMapperProfiles()
        {
            CreateMap<Auth, AuthResponse>().ReverseMap();
        }
    }
}
