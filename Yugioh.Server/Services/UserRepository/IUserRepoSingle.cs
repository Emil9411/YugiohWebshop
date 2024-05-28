using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoSingle
    {
        ActionResult<User> GetUserByEmail(string email);
        ActionResult<User> GetUserById(string id);
        ActionResult<User> AddAdminUser(User user);
        ActionResult<AuthResult> UpdateUser(UpdatePersonalDataRequest updatePersonalDataRequest);
        ActionResult<AuthResult> DeleteUser(AuthRequest authRequest);
    }
}
