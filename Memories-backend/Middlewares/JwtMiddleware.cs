using Memories_backend.Services.Interfaces;

namespace Memories_backend.Middlewares
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly IJwtSecurityTokenService _jwtSecurityTokenHandler;

        public JwtMiddleware(IJwtSecurityTokenService jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateJwtToken(token);
                context.User = claimsPrincipal;
            }

            await next(context);
        }
    }
}
