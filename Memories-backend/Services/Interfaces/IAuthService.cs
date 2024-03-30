using Memories_backend.Models.DTO.Identity.Roles;
using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Memories_backend.Services
{
    public interface IAuthService
    {
        Task SeedRolesAsync();
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
