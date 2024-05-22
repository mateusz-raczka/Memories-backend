using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IUserContextService
    {
        UserContext Current { get; }
        void UpdateUserContext(UserContext userContext);
    }
}
