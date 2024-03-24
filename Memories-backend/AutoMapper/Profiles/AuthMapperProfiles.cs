using AutoMapper;
using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;

namespace Memories_backend.AutoMapper.Profiles
{
    public class AuthMapperProfiles : Profile
    {
        public AuthMapperProfiles() 
        {
            CreateMap<RegisterDto, LoginDto>().ReverseMap();
        }
    }
}
