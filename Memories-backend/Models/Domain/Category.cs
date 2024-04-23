using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Memories_backend.Models.Domain
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        [JsonIgnore]
        public List<File> Files { get; set; }
    }
}
