using Memories_backend.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memories_backend.Models.Domain
{
    public class Folder : IOwnerId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; } 
        public Guid OwnerId { get; set; }

        // Navigation properties
        public FolderDetails FolderDetails { get; set; }
        public Folder ParentFolder { get; set; }
        public IEnumerable<Folder> ChildFolders { get; set; }
        public IEnumerable<File> Files { get; set; }

        void IOwnerId.SetOwnerId(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
