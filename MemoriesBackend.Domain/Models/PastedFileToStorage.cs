using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Models
{
    public class PastedFileToStorage
    {
        public File CopiedFile { get; set; }
        public Guid PastedFileId { get; set; }
    }
}
