using MemoriesBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MemoriesBackend.API.Middlewares
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(
            RequestDelegate next
        )
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<SessionValidationMiddleware> _logger, UserManager<ExtendedIdentityUser> userManager)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userName = context.User.Identity.Name;
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await userManager.FindByIdAsync(userId);

                if (user == null || !user.isLoggedIn)
                {
                    _logger.LogWarning("Unauthorized access attempt by user: {UserName}", userName);
                    throw new UnauthorizedAccessException("User is not logged in.");
                }
            }

            await _next(context);
        }
    }

    public static class SessionValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionValidationMiddleware(
            this IApplicationBuilder builder
            )
        {
            return builder.UseMiddleware<SessionValidationMiddleware>();
        }
    }
}
