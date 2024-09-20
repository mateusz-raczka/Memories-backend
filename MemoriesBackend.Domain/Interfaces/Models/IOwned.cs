namespace MemoriesBackend.Domain.Interfaces.Models
{
    public interface IOwned
    {
        Guid OwnerId { get; set; }
    }
}
