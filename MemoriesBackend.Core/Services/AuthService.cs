using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemoriesBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ExtendedIdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUserContextService _userContextService;

    public AuthService(
        UserManager<ExtendedIdentityUser> userManager,
        ITokenService tokenService,
        IUserContextService userContextService
    )
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userContextService = userContextService;
    }

    public async Task<Auth> LoginAsync(Login login)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);

        if (user == null)
        {
            throw new ApplicationException("User not found");
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, login.Password);

        if (!isPasswordCorrect)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken.Value;
        user.RefreshTokenExpiry = refreshToken.ExpireDate;
        user.isLoggedIn = true;

        try
        {
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new ApplicationException("Failed to update user");
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
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

    public async Task LogoutAsync()
    {
        var userData = _userContextService.Current.UserData;

        var userId = userData.Id;

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new ApplicationException("User not found");
        }

        user.isLoggedIn = false;
        user.RefreshToken = "";
        user.RefreshTokenExpiry = DateTime.MinValue;

        try
        {
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new ApplicationException("Failed to update user");
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ApplicationException("Concurrency error updating user");
        }
    }
}
