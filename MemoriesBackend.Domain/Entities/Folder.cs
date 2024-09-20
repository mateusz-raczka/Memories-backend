using MemoriesBackend.Domain.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class Folder : IOwned, IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid OwnerId { get; set; }
        public HierarchyId HierarchyId { get; set; }
        public HierarchyId? OldHierarchyId { get; set; }

        // Navigation properties
        public virtual FolderDetails FolderDetails { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder ParentFolder { get; set; }
        public virtual List<Folder> ChildFolders { get; set; }
        public virtual List<File> Files { get; set; }
    }
}
