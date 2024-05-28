using System.IdentityModel.Tokens.Jwt;
using Yugioh.Server.Services.AuthServices.Models;

namespace Yugioh.Server.Services.AuthServices.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string emial, string username, string password, string role);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<AuthResult> ChangeEmailAsync(string email, string newEmail);
        JwtSecurityToken Verify(string token);
    }
}
