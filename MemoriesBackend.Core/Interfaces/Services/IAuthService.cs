using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.Authorization;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Auth> LoginAsync(Login login);
    }
}
