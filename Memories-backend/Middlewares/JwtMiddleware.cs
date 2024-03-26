using Memories_backend.Utilities.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Memories_backend.Middlewares
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly JwtSecurityTokenHandlerWrapper _jwtSecurityTokenHandler;

        public JwtMiddleware(JwtSecurityTokenHandlerWrapper jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!token.IsNullOrEmpty())
            {
                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateJwtToken(token);

                var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;

                context.Items["UserId"] = userId;
                context.Items["UserName"] = userName;
            }

            await next(context);
        }
    }
}