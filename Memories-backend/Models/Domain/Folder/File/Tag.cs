using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Memories_backend.Models.Domain.Folder.File
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public List<File> Files { get; set; }
    }
}
