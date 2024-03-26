using AutoMapper;
using Memories_backend.Models.DTO.Identity.Roles;
using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;
using Memories_backend.Utilities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Memories_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly JwtSecurityTokenHandlerWrapper _jwtSecurityTokenHandlerWrapper;
        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            JwtSecurityTokenHandlerWrapper jwtSecurityTokenHandlerWrapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _jwtSecurityTokenHandlerWrapper = jwtSecurityTokenHandlerWrapper;
        }

        public async Task SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
                throw new ApplicationException("Role seeding was already done");

            await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));
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

            await _userManager.AddToRoleAsync(newUser, UserRoles.USER);

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

            var token = _jwtSecurityTokenHandlerWrapper.GenerateJwtToken(user.Id, UserRoles.USER);

            return token;
        }
    }
}
