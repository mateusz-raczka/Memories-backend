using MemoriesBackend.Domain.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class Folder : IOwnerId, IEntity
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
        public List<Folder>? ChildFolders { get; set; }
        public List<File>? Files { get; set; }

        void IOwnerId.SetOwnerId(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
