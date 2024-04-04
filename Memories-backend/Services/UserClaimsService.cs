using System.Security.Claims;
using Memories_backend.Utilities.Helpers;

namespace Memories_backend.Services
{
    public class UserClaimsService : IUserClaimsService
    {
        public Guid UserId { get; }
        public string UserName { get; }

        public UserClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext?.User?.Claims != null)
            {
                Claim userIdClaim = httpContextAccessor.HttpContext.User.Claims
                    .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    UserId = TypeConversion.ConvertStringToGuid(userIdClaim.Value);
                }

                Claim userNameClaim = httpContextAccessor.HttpContext.User.Claims
                    .SingleOrDefault(x => x.Type == ClaimTypes.Name);

                if (userNameClaim != null)
                {
                    UserName = userNameClaim.Value;
                }
            }
        }
    }
}
