using Memories_backend.Models.DTO.Register;

namespace Memories_backend.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
    }
}
