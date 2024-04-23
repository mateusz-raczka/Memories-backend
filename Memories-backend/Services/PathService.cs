using Memories_backend.Repositories;
using Memories_backend.Services.Interfaces;

namespace Memories_backend.Services
{
    public class PathService : IPathService
    {
        private readonly IFolderRepository _folderRepository;

        public PathService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<string> GetFolderPath(Guid folderId)
        {
            var folderPath = new List<Guid>();
            await BuildFolderPath(folderId, folderPath);
            folderPath.Reverse();
            return string.Join("/", folderPath);
        }

        private async Task BuildFolderPath(Guid folderId, List<Guid> folderPath)
        {
            var folder = await _folderRepository.GetById(folderId);
            if (folder != null)
            {
                folderPath.Add(folder.Id);
                if (folder.FolderId != null)
                {
                    await BuildFolderPath(folder.FolderId.Value, folderPath);
                }
            }
        }
    }
}
