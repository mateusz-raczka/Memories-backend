using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IRegisterService
    {
        Task<Auth> RegisterAsync(Register register);
    }
}
