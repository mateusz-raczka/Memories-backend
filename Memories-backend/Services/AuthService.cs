using AutoMapper;
using Memories_backend.Utilities.Authentication.Roles;
using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;
using Microsoft.AspNetCore.Identity;

namespace Memories_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtSecurityTokenService _jwtSecurityTokenHandlerWrapper;
        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IJwtSecurityTokenService jwtSecurityTokenHandlerWrapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtSecurityTokenHandlerWrapper = jwtSecurityTokenHandlerWrapper;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            IdentityUser isExistingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isExistingUser != null)
                throw new ApplicationException("Username already exists");

            IdentityUser newUser = new IdentityUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult createdUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createdUserResult.Succeeded)
            {
                string errorString = "User creation failed because: ";

                foreach (var error in createdUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }

                throw new ApplicationException(errorString);
            }

            await _userManager.AddToRoleAsync(newUser, CustomUserRoles.USER);

            LoginDto loginDto = _mapper.Map<LoginDto>(registerDto);

            string token = await LoginAsync(loginDto);

            return token;
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
