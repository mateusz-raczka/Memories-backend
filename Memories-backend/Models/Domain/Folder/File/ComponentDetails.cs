using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Memories_backend.Models.Domain.Folder.File
{
    public class ComponentDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public Guid Path { get; set; }
        public string Description { get; set; }
        public bool IsStared { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastOpenedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Navigation properties
        public File File { get; set; }
        public Folder Folder { get; set; }
    }
}
