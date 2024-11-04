using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class FileActivity : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ActivityTypeId { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ActivityTypeId))]
        public ActivityType ActivityType { get; set; }
        public File File { get; set; }
    }
}
