using System.ComponentModel.DataAnnotations;

namespace Memories_backend.Utilities.Authorization.DataAuthorize
{
    public class OwnerIdBase : IOwnerId
    {
        [Required]
        [MaxLength(40)]
        public string OwnerId { get; private set; }

        public void SetOwnerId(string protectKey)
        {
            OwnerId = protectKey;
        }
    }
}
