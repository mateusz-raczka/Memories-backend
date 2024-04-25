using Memories_backend.Contexts;
using Memories_backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Memories_backend.Repositories
{
    public class FolderRepository : SQLRepository<Folder>, IFolderRepository
    {
        public FolderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Folder> FindRootFolderAsync() =>
            await _context.Folders.FirstOrDefaultAsync(f => f.FolderId == null);
    }
}
