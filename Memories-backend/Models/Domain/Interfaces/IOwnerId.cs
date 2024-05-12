namespace Memories_backend.Models.Domain.Interfaces
{
    public interface IOwnerId
    {
        Guid OwnerId { get; }
        void SetOwnerId(Guid protectKey);
    }
}
