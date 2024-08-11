using MemoriesBackend.Domain.Models.Authentication;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface ICookieService
    {
        void SetAuthCookies(Auth auth);
        string GetRefreshTokenFromCookie();
        string GetAccessTokenFromCookie();
    }
}
