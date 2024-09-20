using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class FolderDetails : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public string Name { get; set; }
        public bool? IsStared { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastOpenedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid OwnerId { get; set; }

        //Navigation properties
        [ForeignKey(nameof(FolderId))]
        public virtual Folder Folder { get; set; }
    }
}
