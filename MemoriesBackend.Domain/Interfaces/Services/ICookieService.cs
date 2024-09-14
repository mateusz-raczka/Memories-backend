using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface ICookieService
    {
        void SetAuthCookies(Auth auth);
        string GetRefreshTokenFromCookie();
        string GetAccessTokenFromCookie();
    }
}
