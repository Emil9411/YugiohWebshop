using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Context;
using Yugioh.Server.Model.UserModels;

namespace Yugioh.Server.Services.UserRepository
{
    public class UserRepo : IUserRepoSingle, IUserRepoMultiple
    {
        private readonly UsersContext _context;
        private readonly ILogger<UserRepo> _logger;

        public UserRepo(UsersContext context, ILogger<UserRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public ActionResult<User> GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: GetUserByEmail: User with email {email} not found");
                return new NotFoundResult();
            }
            _logger.LogInformation($"UserRepo: GetUserByEmail: User with email {email} found");
            return new OkObjectResult(user);
        }

        public ActionResult<User> GetUserById(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: GetUserById: User with id {id} not found");
                return new NotFoundResult();
            }
            _logger.LogInformation($"UserRepo: GetUserById: User with id {id} found");
            return new OkObjectResult(user);
        }

        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _context.Users.ToList();
            if (users.Count == 0)
            {
                _logger.LogWarning("UserRepo: GetUsers: No users found");
                return new NotFoundResult();
            }
            _logger.LogInformation("UserRepo: GetUsers: Users found");
            return new OkObjectResult(users);
        }

        public ActionResult<User> AddAdminUser(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("UserRepo: AddAdminUser: User is null");
                return new BadRequestResult();
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            _logger.LogInformation($"UserRepo: AddAdminUser: Admin with email {user.Email} added");
            return new OkObjectResult(user);
        }

        public ActionResult<User> UpdateUser(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("UserRepo: UpdateUser: User is null");
                return new BadRequestResult();
            }
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userToUpdate == null)
            {
                _logger.LogWarning($"UserRepo: UpdateUser: User with id {user.Id} not found");
                return new NotFoundResult();
            }
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Country = user.Country;
            userToUpdate.City = user.City;
            userToUpdate.Address = user.Address;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            _context.SaveChanges();
            _logger.LogInformation($"UserRepo: UpdateUser: User with id {user.Id} updated");
            return new OkObjectResult(userToUpdate);
        }

        public ActionResult<User> DeleteUser(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("UserRepo: DeleteUser: User is null");
                return new BadRequestResult();
            }
            var userToDelete = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userToDelete == null)
            {
                _logger.LogWarning($"UserRepo: DeleteUser: User with id {user.Id} not found");
                return new NotFoundResult();
            }
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            _logger.LogInformation($"UserRepo: DeleteUser: User with id {user.Id} deleted");
            return new OkObjectResult(userToDelete);
        }
    }
}
