using Yugioh.Server.Model.UserModels;

namespace Yugioh.Server.Services.UserRepository
{
    public interface IUserRepoMultiple
    {
        Task<IEnumerable<User>?> GetUsersAsync();
    }
}
