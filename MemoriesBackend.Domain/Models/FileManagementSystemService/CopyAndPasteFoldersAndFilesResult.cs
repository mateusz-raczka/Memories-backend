using MemoriesBackend.Domain.Entities;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Domain.Models.FileManagement
{
    public class CopyAndPasteFoldersAndFilesResult
    {
        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}
