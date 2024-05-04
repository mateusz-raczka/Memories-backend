using Memories_backend.Contexts;
using Memories_backend.Models.Domain;
using Memories_backend.Models.DTO.Folder.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Memories_backend.Repositories
{
    public class FolderRepository : SQLRepository<Folder>, IFolderRepository
    {
        public FolderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Folder> FindRootFolderAsync() =>
            await _context.Folders.FirstOrDefaultAsync(f => f.FolderId == null);

        public async Task<IEnumerable<FolderHierarchy>> GetFolderHierarchyAsync(Guid id)
        {
            var folderIdParameter = new SqlParameter("@FolderId", id);

            var foldersHierarchy = await _context.Database.SqlQueryRaw<FolderHierarchy>(
                "EXEC GetFolderHierarchy @FolderId",
                folderIdParameter
            ).ToListAsync();

            return foldersHierarchy;
        }
    }
}
