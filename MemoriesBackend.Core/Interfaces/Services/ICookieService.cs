using MemoriesBackend.Domain.Models.Authorization;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface ICookieService
    {
        void SetAuthCookies(Auth auth);
    }
}
