using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class FileDetails : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string? Description { get; set; }
        public bool? IsStared { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastOpenedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        //Navigation properties
        public File File { get; set; }
    }
}
