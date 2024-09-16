using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Auth> LoginAsync(Login login);
        Task LogoutAsync();
    }
}
