using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class ShareFile : IOwned, IEntity
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public Guid? SharedForUserId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(FileId))]
        public File File { get; set; }
    }
}
