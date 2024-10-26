using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class File : IOwned, IEntity
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(FolderId))]
        public Folder Folder { get; set; }
        public List<Tag>? Tags { get; set; }
        public Category? Category { get; set; }
        public FileDetails FileDetails { get; set; }
        public List<FileActivity>? FileActivities { get; set; }
    }
}
