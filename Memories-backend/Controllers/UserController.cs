using System.IdentityModel.Tokens.Jwt;
using Memories_backend.Models.DTO.Login;
using Memories_backend.Models.DTO.Register;
using Memories_backend.Services;
using Memories_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memories_backend.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRegisterService _registerService;
       
        public UserController(
            IAuthService authService,
            IRegisterService registerService
            )
        {
            _authService = authService;
            _registerService = registerService;
        }

        [HttpPost]
        public async Task Register([FromBody] RegisterDto registerDto)
        {
            string token = await _registerService.RegisterAsync(registerDto);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                Expires = expiration,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            });
        }

        [HttpPost]
        public async Task Login([FromBody] LoginDto loginDto)
        {
           string token = await _authService.LoginAsync(loginDto);
           
           var tokenHandler = new JwtSecurityTokenHandler();
           var jwtToken = tokenHandler.ReadJwtToken(token);
           var expiration = jwtToken.ValidTo;

           Response.Cookies.Append("jwt", token, new CookieOptions
           {
               Expires = expiration,
               HttpOnly = true,
               SameSite = SameSiteMode.Strict,
           });
        }
    }
}
