using MemoriesBackend.Domain.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class Folder : IOwnerId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid OwnerId { get; set; }
        public HierarchyId HierarchyId { get; set; }
        public HierarchyId? OldHierarchyId { get; set; }

        // Navigation properties
        public FolderDetails FolderDetails { get; set; }
        public Folder ParentFolder { get; set; }
        public IEnumerable<Folder>? ChildFolders { get; set; }
        public IEnumerable<File>? Files { get; set; }

        void IOwnerId.SetOwnerId(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
