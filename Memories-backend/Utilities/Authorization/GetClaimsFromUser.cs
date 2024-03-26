using System.Security.Claims;
using Memories_backend.Utilities.Helpers;

namespace Memories_backend.Utilities.Authorization
{
    public class GetClaimsFromUser : IGetClaimsProvider
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }

        public GetClaimsFromUser(IHttpContextAccessor accessor)
        {
            UserId = accessor.HttpContext?
                .User.Claims.SingleOrDefault(x =>
                    x.Type == ClaimTypes.NameIdentifier)?.Value;

            UserName = accessor.HttpContext?
                .User.Claims.SingleOrDefault(x =>
                    x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
