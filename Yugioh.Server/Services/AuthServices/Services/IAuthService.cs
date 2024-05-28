using System.IdentityModel.Tokens.Jwt;
using Yugioh.Server.Services.AuthServices.Models;

namespace Yugioh.Server.Services.AuthServices.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string emial, string username, string password, string role);
        Task<AuthResult> LoginAsync(string email, string password);
        JwtSecurityToken Verify(string token);
    }
}
