using Yugioh.Server.Model;

namespace Yugioh.Server.Services.AuthServices.Services
{
    public interface ITokenService
    {
        string CreateToken(User user, string role);
    }
}
