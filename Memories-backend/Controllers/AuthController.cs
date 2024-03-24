using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;
using Memories_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memories_backend.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task SeedRoles()
        {
            await _authService.SeedRolesAsync();
        }

        [HttpPost]
        public async Task<string> Register([FromBody] RegisterDto registerDto)
        {
            string token = await _authService.RegisterAsync(registerDto);
        
            return token;
        }

        [HttpPost]
        public async Task<string> Login([FromBody] LoginDto loginDto)
        {
           string token = await _authService.LoginAsync(loginDto);

           return token;
        }
    }
}
