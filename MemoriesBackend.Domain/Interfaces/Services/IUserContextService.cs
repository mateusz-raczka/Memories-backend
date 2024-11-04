using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IUserContextService
    {
        UserContext Current { get; }
        void UpdateUserContext(UserContext userContext);
    }
}
