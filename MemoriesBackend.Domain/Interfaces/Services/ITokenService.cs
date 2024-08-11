using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.Tokens;
using System.Security.Claims;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        JwtToken GenerateJwtToken(ExtendedIdentityUser user);
        RefreshToken GenerateRefreshToken();
        ClaimsPrincipal ValidateJwtToken(string token);
        Task<Auth> RefreshToken(string refreshToken, string accessToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    }
}
