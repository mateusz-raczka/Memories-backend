using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesBackend.Domain.Entities
{
    public class FileChunk : IOwned, IEntity
    {
        public Guid Id { get; set; }
        public Guid FileUploadProgressId { get; set; }
        public long Size { get; set; }
        public int ChunkIndex { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(FileUploadProgressId))]
        public FileUploadProgress FileUploadProgress { get; set; }
    }
}
