using MemoriesBackend.API.DTO.Authorization.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using MemoriesBackend.API.DTO.Tokens.Request;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Models.Tokens;

namespace MemoriesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;
        private readonly IMapper _mapper;
        public TokenController(
            ITokenService tokenService,
            IMapper mapper,
            ICookieService cookieService
        )
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _cookieService = cookieService;
        }

        [HttpPost("refresh")]
        public async Task<AuthResponse> Refresh(RefreshTokenRequest refreshToken)
        {
            var refreshTokenDomain = _mapper.Map<RefreshToken>(refreshToken);

            var authDomain = await _tokenService.RefreshToken(refreshTokenDomain);

            var response = _mapper.Map<AuthResponse>(authDomain);

            _cookieService.SetAuthCookies(authDomain);

            return response;
        }
    }
}
