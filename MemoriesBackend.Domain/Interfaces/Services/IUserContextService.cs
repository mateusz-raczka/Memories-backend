using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IUserContextService
    {
        UserContext Current { get; }
        void UpdateUserContext(UserContext userContext);
    }
}
