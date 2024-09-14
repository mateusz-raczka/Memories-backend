using MemoriesBackend.Domain.Entities;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Models
{
    public class FoldersAndFiles
    {
        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}
