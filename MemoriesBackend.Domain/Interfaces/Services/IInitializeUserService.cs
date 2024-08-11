using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IInitializeUserService
    {
        Task InitializeUser(UserContext userContext);
    }
}
