using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class Category : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public IEnumerable<File> Files { get; set; }
    }
}
