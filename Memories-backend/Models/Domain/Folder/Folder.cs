using Memories_backend.Models.Domain.Folder.File;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memories_backend.Models.Domain.Folder
{
    public class Folder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        public ComponentDetails FolderDetails { get; set; }
        public Folder ParentFolder { get; set; }
        public List<Folder> ChildFolders { get; set; }
        public List<File.File> Files { get; set; }
    }
}
