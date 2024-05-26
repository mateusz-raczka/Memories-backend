using MemoriesBackend.Domain.Models.Tokens;
using System.Security.Claims;
using MemoriesBackend.Domain.Entities.Authorization;
using MemoriesBackend.Domain.Models.Authentication;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface ITokenService
    {
        JwtToken GenerateJwtToken(ExtendedIdentityUser user);
        RefreshToken GenerateRefreshToken();
        ClaimsPrincipal ValidateJwtToken(string token);
        Task<Auth> RefreshToken(string refreshToken, string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    }
}
