using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Memories_backend.Services.Interfaces;
using Memories_backend.Utilities.Authentication.Roles;
using Microsoft.IdentityModel.Tokens;

namespace Memories_backend.Services
{
    public class JwtSecurityTokenService : IJwtSecurityTokenService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration;
        private readonly string[] _roles = { 
            CustomUserRoles.OWNER,
            CustomUserRoles.ADMIN,
            CustomUserRoles.USER
        };

        public JwtSecurityTokenService(IConfiguration configuration)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId, string role)

        {
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));

            if(!_roles.Contains(role))
            {
                throw new UnauthorizedAccessException("Invalid role.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
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
    }
}
