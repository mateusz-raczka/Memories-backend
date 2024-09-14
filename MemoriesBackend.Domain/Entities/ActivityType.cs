using MemoriesBackend.Domain.Interfaces.Models;

namespace MemoriesBackend.Domain.Entities
{
    public class ActivityType : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
