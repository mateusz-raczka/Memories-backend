using Memories_backend.Utilities.Helpers;
using System.Security.Claims;

namespace Memories_backend.Models.Authentication
{
    public class UserClaimsValues
    {
        public Guid UserId { get; }

        public UserClaimsValues(ClaimsPrincipal userClaims) 
        {
            Claim userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);

            if(userIdClaim != null)
            {
                UserId = TypeConversion.ConvertStringToGuid(userIdClaim.Value);
            }
        }

        public UserClaimsValues()
        {
            UserId = Guid.Empty;
        }
    }
}
