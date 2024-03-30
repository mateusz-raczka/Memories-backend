namespace Memories_backend.Models.Domain
{
    public interface IOwnerId
    {
        Guid OwnerId { get; }
        void SetOwnerId(Guid protectKey);
    }
}
