using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MemoriesBackend.API.Middlewares
{
    public class SessionValidationMiddleware : IMiddleware
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly ILogger<SessionValidationMiddleware> _logger;

        public SessionValidationMiddleware(
            ILogger<SessionValidationMiddleware> logger,
            UserManager<ExtendedIdentityUser> userManager
            )
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userName = context.User.Identity.Name;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null || !user.isLoggedIn)
                {
                    _logger.LogWarning("Unauthorized access attempt by user: {UserName}", userName);
                    throw new UnauthorizedAccessException("User is not logged in.");
                }
            }

            await next(context);
        }
    }
}
