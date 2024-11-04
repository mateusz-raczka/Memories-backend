using MemoriesBackend.Domain.Models;

namespace MemoriesBackend.Domain.Interfaces.Services
{
    public interface IRegisterService
    {
        Task<Auth> RegisterAsync(Register register);
    }
}
