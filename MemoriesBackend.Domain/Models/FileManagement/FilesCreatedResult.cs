using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Models.FileManagement
{
    public class FilesCreatedResult
    {
        public IEnumerable<File> Files { get; set; }
        public long TotalSize { get; set; }
    }
}
