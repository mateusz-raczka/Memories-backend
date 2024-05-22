using AutoMapper;
using MemoriesBackend.API.DTO.Tokens.Request;
using MemoriesBackend.API.DTO.Tokens.Response;
using MemoriesBackend.Domain.Models.Tokens;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class TokenMapperProfiles : Profile
    {
        public TokenMapperProfiles()
        {
            CreateMap<JwtToken, JwtTokenResponse>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenResponse>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenRequest>().ReverseMap();
        }
    }
}
