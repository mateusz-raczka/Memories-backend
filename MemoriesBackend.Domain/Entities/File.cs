using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class File : IOwned, IEntity
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        [Required]
        [ForeignKey(nameof(FolderId))]
        public virtual Folder Folder { get; set; }
        public virtual List<Tag>? Tags { get; set; }
        public virtual Category? Category { get; set; }
        public virtual FileDetails FileDetails { get; set; }
        public virtual List<FileActivity>? FileActivities { get; set; }
    }
}
