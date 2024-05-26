using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Entities.Authorization;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ExtendedIdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ExtendedIdentityUser> userManager,
        ITokenService tokenService,
        ILogger<AuthService> logger
    )
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Auth> LoginAsync(Login loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            _logger.LogWarning("User not found: {UserName}", loginDto.UserName);
            throw new ApplicationException("User not found");
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isPasswordCorrect)
        {
            _logger.LogWarning("Invalid credentials for user: {UserName}", loginDto.UserName);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken.Value;
        user.RefreshTokenExpiry = refreshToken.ExpireDate;

        try
        {
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _logger.LogError("Error updating user: {UserName}, Errors: {Errors}",
                    loginDto.UserName, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                throw new ApplicationException("Failed to update user");
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError("Concurrency error updating user: {UserName}, Error: {Error}",
                loginDto.UserName, ex.Message);
            throw new ApplicationException("Concurrency error updating user");
        }

        var userData = new UserData()
        {
            Id = Guid.Parse(user.Id),
            Name = user.UserName,
            Email = user.Email
        };

        var auth = new Auth()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            User = userData
        };

        return auth;
    }
}
