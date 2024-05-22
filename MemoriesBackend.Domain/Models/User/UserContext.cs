using System.Security.Claims;

namespace MemoriesBackend.Domain.Models.User
{
    public class UserContext
    {
        public UserData UserData { get; set; }
        public ClaimsPrincipal UserClaims { get; set; }
    }
}
