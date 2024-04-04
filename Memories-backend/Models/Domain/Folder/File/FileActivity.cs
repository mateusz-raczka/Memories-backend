using System.ComponentModel.DataAnnotations.Schema;

namespace Memories_backend.Models.Domain.Folder.File
{
    public class FileActivity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        public ActivityType ActivityType { get; set; }
    }
}
