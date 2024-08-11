using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Domain.Models.Authentication;
using MemoriesBackend.Domain.Models.Tokens;
using MemoriesBackend.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MemoriesBackend.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly IUserContextService _userContextService;

        private readonly string[] _roles =
        {
            ApplicationRoles.OWNER,
            ApplicationRoles.ADMIN,
            ApplicationRoles.USER
        };

        public TokenService(
            IConfiguration configuration,
            IUserContextService userContextService,
            UserManager<ExtendedIdentityUser> userManager
            )
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
            _userContextService = userContextService;
            _userManager = userManager;
        }

        public JwtToken GenerateJwtToken(ExtendedIdentityUser user)
        {
            if (user is null)
                throw new ApplicationException("Invalid user data - Cannot generate jwt token");

            var userRole = ApplicationRoles.USER;

            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));

            if (!_roles.Contains(ApplicationRoles.USER)) throw new UnauthorizedAccessException("Invalid role.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, userRole)
            };

            var expireDate = DateTime.UtcNow.AddHours(2);

            var identity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = identity,
                Expires = expireDate,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtSecurityToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var token = new JwtToken()
            {
                ExpireDate = expireDate,
                Value = _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken)
            };

            return token;
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

            try
            {
                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new UnauthorizedAccessException("Token has expired.");
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }
        }

        public RefreshToken GenerateRefreshToken()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

            var length = 64;

            var randomNumber = new byte[length];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            var tokenBuilder = new StringBuilder(length);

            foreach (var b in randomNumber)
            {
                tokenBuilder.Append(chars[b % chars.Length]);
            }

            var refreshToken = new RefreshToken()
            {
                ExpireDate = DateTime.UtcNow.AddDays(14),
                Value = tokenBuilder.ToString()
            };

            return refreshToken;
        }

        public async Task<Auth> RefreshToken(string refreshToken, string accessToken)
        {
            var userPrincipal = GetPrincipalFromExpiredToken(accessToken);

            var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                throw new ApplicationException("Invalid or missing user ID in access token");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new ApplicationException("Refresh token is not valid");

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken.Value;
            user.RefreshTokenExpiry = newRefreshToken.ExpireDate;

            var userUpdateResult = await _userManager.UpdateAsync(user);

            if (!userUpdateResult.Succeeded)
            {
                throw new ApplicationException("Failed to update user with new refresh token");
            }

            var userData = new UserData()
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                Name = user.UserName
            };

            var auth = new Auth()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                User = userData
            };

            return auth;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

            try
            {
                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }
        }
    }
}
