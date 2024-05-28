using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Context;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;

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

        public ActionResult<AuthResult> UpdateUser(UpdatePersonalDataRequest updatePersonalDataRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == updatePersonalDataRequest.Email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: UpdateUser: User with email {updatePersonalDataRequest.Email} not found");
                return new NotFoundResult();
            }
            var properties = typeof(UpdatePersonalDataRequest).GetProperties();
            foreach (var property in properties)
            {
                var newValue = property.GetValue(updatePersonalDataRequest);
                var userProperty = typeof(User).GetProperty(property.Name);
                if (userProperty != null)
                {
                    var currentValue = userProperty.GetValue(user);
                    if (!Equals(currentValue, newValue))
                    {
                        userProperty.SetValue(user, newValue);
                    }
                }
            }
            _context.Users.Update(user);
            _context.SaveChanges();
            _logger.LogInformation($"UserRepo: UpdateUser: User with email {updatePersonalDataRequest.Email} updated");
            return new OkObjectResult(new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", ""));
        }

        public ActionResult<AuthResult> DeleteUser(AuthRequest authRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == authRequest.Email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: DeleteUser: User with email {authRequest.Email} not found");
                return new NotFoundResult();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            _logger.LogInformation($"UserRepo: DeleteUser: User with email {authRequest.Email} deleted");
            return new OkObjectResult(new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", ""));
        }
    }
}
