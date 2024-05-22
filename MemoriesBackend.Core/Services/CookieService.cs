using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Models.Authorization;
using Microsoft.AspNetCore.Http;

namespace MemoriesBackend.Application.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(
            IHttpContextAccessor httpContextAccessor
        )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetAuthCookies(Auth auth)
        {
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = auth.AccessToken.ExpireDate
            };

            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = auth.RefreshToken.ExpireDate
            };

            _httpContextAccessor.HttpContext.Response.Cookies
                .Append("accessToken", auth.AccessToken.Value,
                accessTokenCookieOptions);
            _httpContextAccessor.HttpContext.Response.Cookies
                .Append("refreshToken", auth.RefreshToken.Value, refreshTokenCookieOptions);
        }
    }

}
