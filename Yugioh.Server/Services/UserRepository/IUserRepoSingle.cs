using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoSingle
    {
        Task<UserResponse?> GetUserByEmailAsync(string email);
        Task<AuthResult?> AddAdminUserAsync(RegistrationRequest registrationRequest);
        Task<AuthResult?> UpdateUserAsync(UpdatePersonalDataRequest updatePersonalDataRequest);
        Task<AuthResult?> DeleteUserAsync(AuthRequest authRequest);
        Task<AuthResult?> DeleteUserAdminAsync(string email);
    }
}
