using Memories_backend.Utilities.Authentication.Roles;
using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;
using Microsoft.AspNetCore.Identity;
using Memories_backend.Services.Interfaces;

namespace Memories_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtSecurityTokenService _jwtSecurityTokenHandlerWrapper;
        public AuthService(
            UserManager<IdentityUser> userManager,
            IJwtSecurityTokenService jwtSecurityTokenHandlerWrapper
            )
        {
            _userManager = userManager;
            _jwtSecurityTokenHandlerWrapper = jwtSecurityTokenHandlerWrapper;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            IdentityUser user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _jwtSecurityTokenHandlerWrapper.GenerateJwtToken(user.Id, CustomUserRoles.USER);

            return token;
        }
    }
}
