using Memories_backend.Models.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memories_backend.Models.Domain
{
    public class File : IOwnerId
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        public Folder Folder { get; set; }
        public List<Tag>? Tags { get; set; }
        public Category? Category { get; set; }
        public FileDetails FileDetails { get; set; }
        public List<FileActivity> FileActivities { get; set; }

        void IOwnerId.SetOwnerId(Guid ownerId)
        { 
            OwnerId = ownerId;
        }
    }
}
