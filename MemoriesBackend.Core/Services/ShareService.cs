using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace MemoriesBackend.Application.Services
{
    public class ShareService : IShareService
    {
        private readonly IGenericRepository<ShareFile> _shareFileRepository;
        private readonly UserManager<ExtendedIdentityUser> _userManager;

        public ShareService(IGenericRepository<ShareFile> shareFileRepository)
        {
            _shareFileRepository = shareFileRepository;
        }

        public async Task<ShareFile> ShareFileAsync(ShareFile shareFile)
        {
            if(shareFile == null)
            {
                throw new ApplicationException("Failed to share - share data is null");
            }

            if(shareFile.SharedForUserId != null)
            {
                var userToShare = await _userManager.FindByIdAsync(shareFile.SharedForUserId.ToString());

                if (userToShare == null)
                {
                    throw new ApplicationException($"Failed to share - cannot find a user with given id");
                }
            }

            // TODO: Check if file is already shared with user which you try to share with

            var sharedFile = await _shareFileRepository.Create(shareFile);

            return sharedFile;
        }
    }
}
