using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Application.Services
{
    // write documentation



    public class UserContextService : IUserContextService
    {
        public UserContext Current { get; private set; }

        public UserContextService(
            IHttpContextAccessor httpContextAccessor
        )
        {
            Current = GetUserRequestContext(httpContextAccessor);
        }

        public void UpdateUserContext(UserContext userContext) =>
            Current = userContext ?? throw new ArgumentNullException(nameof(userContext));

        private UserContext GetUserRequestContext(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor?.HttpContext;

            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var user = httpContext.User;

            var userData = new UserData()
            {
                Id = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                Name = user.Identity.Name
            };

            var userContext = new UserContext
            {
                UserData = userData,
                UserClaims = user
            };

            return userContext;
        }
    }
}