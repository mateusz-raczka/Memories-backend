using System.ComponentModel.DataAnnotations.Schema;

namespace Memories_backend.Models.Domain
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public IEnumerable<File> Files { get; set; }
    }
}
