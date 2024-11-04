using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IInitializeUserService
    {
        Task InitializeUser(UserContext userContext);
    }
}
