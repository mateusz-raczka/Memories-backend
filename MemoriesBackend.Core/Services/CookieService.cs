using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.Tokens;
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
            SetRefreshTokenCookie(auth.RefreshToken);
            SetAccessTokenCookie(auth.AccessToken);
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

        private void SetAccessTokenCookie(JwtToken accessToken)
        {
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = accessToken.ExpireDate
            };

            _httpContextAccessor.HttpContext.Response.Cookies
                .Append(CookieType.accessToken.ToString(), accessToken.Value, accessTokenCookieOptions);
        }

        private void SetRefreshTokenCookie(RefreshToken refreshToken)
        {
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpireDate
            };

            _httpContextAccessor.HttpContext.Response.Cookies
                .Append(CookieType.refreshToken.ToString(), refreshToken.Value, refreshTokenCookieOptions);
        }
    }

}
