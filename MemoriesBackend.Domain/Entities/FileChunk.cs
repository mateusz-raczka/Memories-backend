using MemoriesBackend.Domain.Interfaces.Models;

namespace MemoriesBackend.Domain.Entities
{
    public class FileChunk : IOwnerId, IEntity
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public long Size { get; set; }
        public int ChunkIndex { get; set; }
        public Guid OwnerId { get; set; }

        public void SetOwnerId(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
