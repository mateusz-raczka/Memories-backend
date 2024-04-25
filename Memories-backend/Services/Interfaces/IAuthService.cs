using Memories_backend.Models.DTO.Login;

namespace Memories_backend.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
