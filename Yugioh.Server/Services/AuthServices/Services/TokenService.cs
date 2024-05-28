
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Yugioh.Server.Model;
using Yugioh.Server.Services.AuthServices.Models;

namespace Yugioh.Server.Services.AuthServices.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private const int ExpirationInMinutes = 60;

        public TokenService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string CreateToken(User user, string role)
        {
            try
            {
                var claims = CreateClaims(user, role);
                var signingCredentials = CreateSigningCredentials();
                var expiration = DateTime.UtcNow.AddMinutes(ExpirationInMinutes);
                var jwtToken = CreateJwtToken(claims, signingCredentials, expiration);
                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
            catch
            {
                _logger.LogError("TokenService: Error creating token");
                return string.Empty;
            }
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials signingCredentials, DateTime expiration)
        {
            return new JwtSecurityToken(
                issuer: _configuration.GetSection("JwtSettings").Get<JwtSettings>()?.ValidIssuer,
                audience: _configuration.GetSection("JwtSettings").Get<JwtSettings>()?.ValidAudience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );
        }

        private List<Claim> CreateClaims(User user, string? role)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email)
                };
                if (!string.IsNullOrEmpty(role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                return claims;
            }
            catch
            {
                _logger.LogError("TokenService: Error creating claims for token");
                return new List<Claim>();
            }
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetSection("IssuerSigningKey").Value)
                ),
                SecurityAlgorithms.HmacSha256
                );
        }
    }
}
