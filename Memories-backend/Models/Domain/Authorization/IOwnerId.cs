namespace Memories_backend.Models.Domain.Authorization
{
    public interface IOwnerId
    {
        Guid OwnerId { get; }
        void SetOwnerId(Guid protectKey);
    }
}
