using Microsoft.AspNetCore.Identity;

namespace MemoriesBackend.Domain.Entities
{
    public class ExtendedIdentityUser : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
