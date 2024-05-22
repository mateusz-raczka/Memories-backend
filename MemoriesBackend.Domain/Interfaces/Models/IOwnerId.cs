namespace MemoriesBackend.Domain.Interfaces.Models
{
    public interface IOwnerId
    {
        Guid OwnerId { get; }
        void SetOwnerId(Guid protectKey);
    }
}
