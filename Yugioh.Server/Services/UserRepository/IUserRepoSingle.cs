using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoSingle
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> AddAdminUserAsync(User user);
        Task<AuthResult?> UpdateUserAsync(UpdatePersonalDataRequest updatePersonalDataRequest);
        Task<AuthResult?> DeleteUserAsync(AuthRequest authRequest);
        Task<AuthResult?> DeleteUserAdminAsync(string email);
    }
}
