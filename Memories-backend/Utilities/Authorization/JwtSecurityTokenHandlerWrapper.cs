using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Memories_backend.Utilities.Authorization
{
    public class JwtSecurityTokenHandlerWrapper
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration;

        public JwtSecurityTokenHandlerWrapper(IConfiguration configuration)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId, string role)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

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
                Expires = DateTime.UtcNow.AddMinutes(30),
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
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new ApplicationException("Token has expired.");
            }
        }
    }
}
