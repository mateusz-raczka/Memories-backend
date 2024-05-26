using MemoriesBackend.Domain.Models.Authentication;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface ICookieService
    {
        void SetAuthCookies(Auth auth);
        string GetRefreshTokenFromCookie();
        string GetAccessTokenFromCookie();
    }
}
