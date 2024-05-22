using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IInitializeUserService
    {
        Task InitializeUser(UserContext userContext);
    }
}
