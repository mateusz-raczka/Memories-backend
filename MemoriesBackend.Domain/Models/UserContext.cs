using System.Security.Claims;

namespace MemoriesBackend.Domain.Models
{
    public class UserContext
    {
        public UserData UserData { get; set; }
        public ClaimsPrincipal UserClaims { get; set; }
    }
}
