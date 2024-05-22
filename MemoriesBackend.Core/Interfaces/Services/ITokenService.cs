﻿using MemoriesBackend.Domain.Models.Authorization;
using MemoriesBackend.Domain.Models.Tokens;
using System.Security.Claims;
using MemoriesBackend.Domain.Entities.Authorization;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface ITokenService
    {
        JwtToken GenerateJwtToken(ExtendedIdentityUser user);
        RefreshToken GenerateRefreshToken();
        ClaimsPrincipal ValidateJwtToken(string token);
        Task<Auth> RefreshToken(RefreshToken refreshToken);
    }
}
