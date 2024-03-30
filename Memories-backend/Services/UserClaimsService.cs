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
            IEnumerable<Claim>? Claims = httpContextAccessor.HttpContext?.User.Claims;

            string? userId = Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if(userId != null)
            {
                UserId = TypeConversion.ConvertStringToGuid(userId);
            }
            
            UserName = Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
