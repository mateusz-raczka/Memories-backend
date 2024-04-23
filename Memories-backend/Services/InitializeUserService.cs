using Memories_backend.Services.Interfaces;

namespace Memories_backend.Services
{
    public class InitializeUserService : IInitializeUserService
    {
        private readonly IFolderDatabaseService _folderDatabaseService;
        private readonly IUserClaimsService _userClaimsService;
        public InitializeUserService(
            IFolderDatabaseService folderDatabaseService,
            IUserClaimsService userClaimsService
            )
        {
            _folderDatabaseService = folderDatabaseService;
            _userClaimsService = userClaimsService;
        }

        public async Task InitializeUser(string token)
        {
            _userClaimsService.UpdateUserClaims(token);

            await _folderDatabaseService.CreateRootFolderAsync();
        }
    }
}
