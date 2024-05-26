using MemoriesBackend.Domain.Models.Authentication;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Auth> LoginAsync(Login login);
    }
}
