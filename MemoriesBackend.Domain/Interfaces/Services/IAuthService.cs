using MemoriesBackend.Domain.Models.Authentication;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Auth> LoginAsync(Login login);
    }
}
