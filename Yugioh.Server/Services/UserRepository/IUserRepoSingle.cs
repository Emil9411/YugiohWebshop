using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Model.UserModels;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoSingle
    {
        ActionResult<User> GetUserByEmail(string email);
        ActionResult<User> GetUserById(string id);
        ActionResult<User> AddAdminUser(User user);
        ActionResult<User> UpdateUser(User user);
        ActionResult<User> DeleteUser(User user);
    }
}
