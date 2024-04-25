using Memories_backend.Models.Authentication;
using Memories_backend.Services.Interfaces;
using System.Security.Claims;

public class UserClaimsService : IUserClaimsService
{
    private readonly IJwtSecurityTokenService _jwtSecurityTokenService;

    public UserClaimsValues UserClaimsValues { get; private set; }

    public UserClaimsService(
        IHttpContextAccessor httpContextAccessor, 
        IJwtSecurityTokenService jwtSecurityTokenService
        )
    {
        _jwtSecurityTokenService = jwtSecurityTokenService;

        UserClaimsValues = GetUserClaimsFromHttpContextAccessor(httpContextAccessor);
    }
    public void UpdateUserClaims(string token)
    {
        ClaimsPrincipal userClaims = _jwtSecurityTokenService.ValidateJwtToken(token);

        UserClaimsValues = new UserClaimsValues(userClaims);
    }

    private UserClaimsValues GetUserClaimsFromHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        var userHttpContextClaims = httpContextAccessor?.HttpContext?.User;

        if (userHttpContextClaims != null)
        {
            return new UserClaimsValues(userHttpContextClaims);
        }

        return new UserClaimsValues();
    }
}
