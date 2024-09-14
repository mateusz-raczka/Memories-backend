using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Request;
using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class RegisterMapperProfiles : Profile
    {
        public RegisterMapperProfiles()
        {
            CreateMap<Register, RegisterRequest>().ReverseMap();
        }
    }
}
