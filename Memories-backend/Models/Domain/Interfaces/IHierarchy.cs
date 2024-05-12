using Microsoft.EntityFrameworkCore;

namespace Memories_backend.Models.Domain.Interfaces
{
    public interface IHierarchy
    {
        HierarchyId HierarchyId { get; set; }
        HierarchyId? OldHierarchyId { get; set; }
    }
}
