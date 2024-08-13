using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace MemoriesBackend.Application.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly IInitializeUserService _initializeUserService;
        private readonly ITransactionHandler _transactionHandler;

        public RegisterService(
            UserManager<ExtendedIdentityUser> userManager,
            IInitializeUserService initializeUserService,
            ITransactionHandler transactionHandler
        )
        {
            _userManager = userManager;
            _initializeUserService = initializeUserService;
            _transactionHandler = transactionHandler;
        }

        public async Task<UserData> RegisterAsync(Register register)
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

                var userContext = new Domain.Models.User.UserContext()
                {
                    UserData = userData
                };

                await _initializeUserService.InitializeUser(userContext);

                return userData;
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