using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IRegisterService
    {
        Task<UserData> RegisterAsync(Register register);
    }
}
