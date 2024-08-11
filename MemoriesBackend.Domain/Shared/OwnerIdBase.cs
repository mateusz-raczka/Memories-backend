using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace MemoriesBackend.Domain.Shared
{
    public class OwnerIdBase : IOwnerId
    {
        [Required]
        [MaxLength(40)]
        public Guid OwnerId { get; private set; }

        public void SetOwnerId(Guid protectKey)
        {
            OwnerId = protectKey;
        }
    }

}