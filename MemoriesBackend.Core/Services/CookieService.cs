using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace MemoriesBackend.Application.Services
{
    enum CookieType
    {
        accessToken,
        refreshToken
    }

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
            SetRefreshTokenCookie(auth.RefreshToken, auth.RefreshToken.ExpireDate);
            SetAccessTokenCookie(auth.AccessToken, auth.RefreshToken.ExpireDate);
        }

        public string GetRefreshTokenFromCookie()
        {
            var refreshTokenValue = _httpContextAccessor.HttpContext.Request.Cookies[CookieType.refreshToken.ToString()];

            if (string.IsNullOrEmpty(refreshTokenValue))
            {
                throw new ArgumentException("Refresh token cookie is empty");
            }

            return refreshTokenValue;
        }

        public string GetAccessTokenFromCookie()
        {
            var accessTokenValue = _httpContextAccessor.HttpContext.Request.Cookies[CookieType.accessToken.ToString()];

            if (string.IsNullOrEmpty(accessTokenValue))
            {
                throw new ArgumentException("Access token cookie is empty");
            }

            return accessTokenValue;
        }

        private void SetAccessTokenCookie(JwtToken accessToken, DateTime expireDate)
        {
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expireDate
            };

            _httpContextAccessor.HttpContext.Response.Cookies
                .Append(CookieType.accessToken.ToString(), accessToken.Value, accessTokenCookieOptions);
        }

        private void SetRefreshTokenCookie(RefreshToken refreshToken, DateTime expireDate)
        {
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expireDate
            };

            _httpContextAccessor.HttpContext.Response.Cookies
                .Append(CookieType.refreshToken.ToString(), refreshToken.Value, refreshTokenCookieOptions);
        }
    }

}
