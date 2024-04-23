using System.Security.Claims;

namespace Memories_backend.Services.Interfaces
{
    public interface IJwtSecurityTokenService
    {
        ClaimsPrincipal ValidateJwtToken(string token);
        string GenerateJwtToken(string userId, string role);
    }
}
