using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class ActivityType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
