using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Response;
using MemoriesBackend.Domain.Models.Authentication;

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
