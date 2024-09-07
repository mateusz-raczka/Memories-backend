using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Response;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.API.Controllers
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

        [HttpGet("refresh")]
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
