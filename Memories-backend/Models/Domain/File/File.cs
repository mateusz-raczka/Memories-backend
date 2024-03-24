using System.ComponentModel.DataAnnotations.Schema;

namespace Memories_backend.Models.Domain
{
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public bool isFolder { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        public List<Tag>? Tags { get; set; }
        public Category? Category { get; set; }
        public FileDetails FileDetails { get; set; }
        public List<FileActivity> FileActivities { get; set; }
    }
}
