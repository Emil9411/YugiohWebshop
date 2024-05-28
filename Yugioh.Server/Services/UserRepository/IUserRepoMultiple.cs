using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Model.UserModels;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoMultiple
    {
        Task<ActionResult<IEnumerable<User>>> GetUsersAsync();
    }
}
