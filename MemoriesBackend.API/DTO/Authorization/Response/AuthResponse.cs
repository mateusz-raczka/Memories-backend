using MemoriesBackend.API.DTO.Tokens.Response;
using MemoriesBackend.API.DTO.User;
using MemoriesBackend.Domain.Models.Authorization;

namespace MemoriesBackend.API.DTO.Authorization.Response
{
    public class AuthResponse
    {
        public UserDataResponse User { get; set; }
        public JwtTokenResponse AccessToken { get; set; }
        public RefreshTokenResponse RefreshToken { get; set; }
    }
}
