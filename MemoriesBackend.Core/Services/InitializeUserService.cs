using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Application.Services
{
    public class InitializeUserService : IInitializeUserService
    {
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly IUserContextService _userClaimsService;

        public InitializeUserService(
            IFolderDatabaseService folderDatabaseService,
            IUserContextService userClaimsService
        )
        {
            _folderDatabaseService = folderDatabaseService;
            _userClaimsService = userClaimsService;
        }

        public async Task InitializeUser(UserContext userContext)
        {
            _userClaimsService.UpdateUserContext(userContext);

            await _folderDatabaseService.CreateRootFolderAsync();
        }
    }
}
