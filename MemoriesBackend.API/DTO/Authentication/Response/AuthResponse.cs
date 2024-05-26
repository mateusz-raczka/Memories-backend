using MemoriesBackend.API.DTO.Tokens.Response;
using MemoriesBackend.API.DTO.User;

namespace MemoriesBackend.API.DTO.Authentication.Response
{
    public class AuthResponse
    {
        public UserDataResponse User { get; set; }
        public JwtTokenResponse AccessToken { get; set; }
        public RefreshTokenResponse RefreshToken { get; set; }
    }
}
