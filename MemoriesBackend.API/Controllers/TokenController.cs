using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Response;
using MemoriesBackend.API.DTO.Tokens.Request;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Models.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.Controllers
{
    [AllowAnonymous]
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
        public async Task<AuthResponse> Refresh()
        {
            var refreshToken = _cookieService.GetRefreshTokenFromCookie();

            var accessToken = _cookieService.GetAccessTokenFromCookie();

            var authDomain = await _tokenService.RefreshToken(refreshToken, accessToken);

            var response = _mapper.Map<AuthResponse>(authDomain);

            _cookieService.SetAuthCookies(authDomain);

            return response;
        }
    }
}
