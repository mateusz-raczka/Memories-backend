using AutoMapper;
using MemoriesBackend.API.DTO.Authentication.Request;
using MemoriesBackend.API.DTO.Authentication.Response;
using MemoriesBackend.API.DTO.User;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.API.Controllers;

[AllowAnonymous]
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRegisterService _registerService;
    private readonly IMapper _mapper;
    private readonly ICookieService _cookieService;

    public UserController(
        IAuthService authService,
        IRegisterService registerService,
        IMapper mapper,
        ICookieService cookieService
    )
    {
        _authService = authService;
        _registerService = registerService;
        _mapper = mapper;
        _cookieService = cookieService;
    }

    [HttpPost]
    public async Task<UserDataResponse> Register([FromBody] RegisterRequest registerDto)
    {
        var registerDomain = _mapper.Map<Register>(registerDto);

        var authDomain = await _registerService.RegisterAsync(registerDomain);

        var response = _mapper.Map<UserDataResponse>(authDomain);

        return response;
    }

    [HttpPost]
    public async Task<AuthResponse> Login([FromBody] LoginResponse loginDto)
    {
        var loginDomain = _mapper.Map<Login>(loginDto);

        var authDomain = await _authService.LoginAsync(loginDomain);

        var response = _mapper.Map<AuthResponse>(authDomain);

        _cookieService.SetAuthCookies(authDomain);

        return response;
    }
}