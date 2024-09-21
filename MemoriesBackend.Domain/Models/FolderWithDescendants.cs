using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Models
{
    public class FolderWithDescendants
    {
        public Folder Folder { get; set; }
        public IEnumerable<Folder> Descendants { get; set; }
    }
}
