namespace Memories_backend.Models.Authorization
{
    public interface IOwnerId
    {
        Guid OwnerId { get; }
        void SetOwnerId(Guid protectKey);
    }
}
