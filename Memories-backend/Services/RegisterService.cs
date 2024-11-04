using Memories_backend.Models.DTO.Register;
using Memories_backend.Services.Interfaces;
using Memories_backend.Utilities.Authentication.Roles;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Memories_backend.Services
{
    public class RegisterService : IRegisterService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtSecurityTokenService _jwtSecurityTokenHandlerWrapper;
        private readonly IInitializeUserService _initializeUserService;

        public RegisterService(
            UserManager<IdentityUser> userManager,
            IJwtSecurityTokenService jwtSecurityTokenHandlerWrapper,
            IInitializeUserService initializeUserService
            )
        {
            _userManager = userManager;
            _jwtSecurityTokenHandlerWrapper = jwtSecurityTokenHandlerWrapper;
            _initializeUserService = initializeUserService;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                IdentityUser newUser = await CreateUserAsync(registerDto);

                string token = _jwtSecurityTokenHandlerWrapper.GenerateJwtToken(newUser.Id, CustomUserRoles.USER);

                await _initializeUserService.InitializeUser(token);

                transaction.Complete();

                return token;
            }
        }

        private async Task<IdentityUser> CreateUserAsync(RegisterDto registerDto)
        {
            IdentityUser newUser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, CustomUserRoles.USER);

                return newUser;
            }
            else
            {
                string errorString = "User creation failed because: ";
                foreach (var error in result.Errors)
                {
                    errorString += " # " + error.Description;
                }
                throw new ApplicationException(errorString);
            }
        }

    }
}
