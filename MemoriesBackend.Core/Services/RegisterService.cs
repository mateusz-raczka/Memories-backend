using System.Transactions;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Entities.Authorization;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.Authorization;
using MemoriesBackend.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace MemoriesBackend.Application.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IInitializeUserService _initializeUserService;

        public RegisterService(
            UserManager<ExtendedIdentityUser> userManager,
            ITokenService tokenService,
            IInitializeUserService initializeUserService
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _initializeUserService = initializeUserService;
        }

        public async Task<Auth> RegisterAsync(Register register)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var newUser = await CreateUserAsync(register);

                var userData = new UserData()
                {
                    Id = Guid.Parse(newUser.Id),
                    Email = newUser.Email,
                    Name = newUser.UserName
                };

                var userContext = new UserContext()
                {
                    UserData = userData
                };

                await _initializeUserService.InitializeUser(userContext);

                var token = _tokenService.GenerateJwtToken(newUser);
                var refreshToken = _tokenService.GenerateRefreshToken();

                var auth = new Auth()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    User = userData
                };

                transaction.Complete();

                return auth;
            }
        }

        private async Task<ExtendedIdentityUser> CreateUserAsync(Register register)
        {
            var newUser = new ExtendedIdentityUser
            {
                UserName = register.UserName,
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, ApplicationRoles.USER);

                return newUser;
            }

            var errorString = "User creation failed because: ";
            foreach (var error in result.Errors) errorString += " # " + error.Description;
            throw new ApplicationException(errorString);
        }
    }
}