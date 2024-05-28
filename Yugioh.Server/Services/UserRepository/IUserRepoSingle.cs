using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoSingle
    {
        Task<ActionResult<User>> GetUserByEmailAsync(string email);
        Task<ActionResult<User>> GetUserByIdAsync(string id);
        Task<ActionResult<User>> AddAdminUserAsync(User user);
        Task<ActionResult<AuthResult>> UpdateUserAsync(UpdatePersonalDataRequest updatePersonalDataRequest);
        Task<ActionResult<AuthResult>> DeleteUserAsync(AuthRequest authRequest);
    }
}
