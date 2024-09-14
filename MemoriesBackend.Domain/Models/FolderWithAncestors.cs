using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Models
{
    public class FolderWithAncestors
    {
        public Folder Folder { get; set; }
        public IEnumerable<Folder> Ancestors { get; set; }
    }
}
