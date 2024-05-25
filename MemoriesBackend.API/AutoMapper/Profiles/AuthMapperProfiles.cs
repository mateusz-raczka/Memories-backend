using AutoMapper;
using MemoriesBackend.API.DTO.Authorization.Response;
using MemoriesBackend.Domain.Models.Authorization;

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
