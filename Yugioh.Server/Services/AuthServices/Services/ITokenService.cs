using Yugioh.Server.Model.UserModels;

namespace Yugioh.Server.Services.AuthServices.Services
{
    public interface ITokenService
    {
        string CreateToken(User user, string role);
    }
}
