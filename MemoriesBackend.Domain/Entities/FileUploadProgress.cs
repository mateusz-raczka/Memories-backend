using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class FileUploadProgress : IOwnerId, IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int TotalChunks { get; set; }
        public string RelativePath { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid OwnerId { get; set; }

        public List<FileChunk> FileChunks { get; set; }

        void IOwnerId.SetOwnerId(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
