using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemoriesBackend.Application.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly IInitializeUserService _initializeUserService;
        private readonly ITransactionHandler _transactionHandler;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegisterService> _logger;

        public RegisterService(
            UserManager<ExtendedIdentityUser> userManager,
            IInitializeUserService initializeUserService,
            ITransactionHandler transactionHandler,
            ITokenService tokenService,
            ILogger<RegisterService> logger
        )
        {
            _userManager = userManager;
            _initializeUserService = initializeUserService;
            _transactionHandler = transactionHandler;
            _tokenService = tokenService;
            _logger = logger;
        }
        
        public async Task<Auth> RegisterAsync(Register register)
        {
            return await _transactionHandler.ExecuteAsync(async () =>
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

                newUser.RefreshToken = refreshToken.Value;
                newUser.RefreshTokenExpiry = refreshToken.ExpireDate;

                try
                {
                    var updateResult = await _userManager.UpdateAsync(newUser);

                    if (!updateResult.Succeeded)
                    {
                        _logger.LogError("Error updating user: {UserName}, Errors: {Errors}",
                            register.UserName, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                        throw new ApplicationException("Failed to update user");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError("Concurrency error updating user: {UserName}, Error: {Error}",
                        register.UserName, ex.Message);
                    throw new ApplicationException("Concurrency error updating user");
                }

                var auth = new Auth()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    User = userData
                };

                return auth;
            });
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