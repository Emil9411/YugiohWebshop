using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ActionResult<User>> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: GetUserByEmail: User with email {email} not found");
                return new NotFoundResult();
            }
            _logger.LogInformation($"UserRepo: GetUserByEmail: User with email {email} found");
            return new OkObjectResult(user);
        }

        public async Task<ActionResult<User>> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: GetUserById: User with id {id} not found");
                return new NotFoundResult();
            }
            _logger.LogInformation($"UserRepo: GetUserById: User with id {id} found");
            return new OkObjectResult(user);
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
            {
                _logger.LogWarning("UserRepo: GetUsers: No users found");
                return new NotFoundResult();
            }
            _logger.LogInformation("UserRepo: GetUsers: Users found");
            return new OkObjectResult(users);
        }

        public async Task<ActionResult<User>> AddAdminUserAsync(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("UserRepo: AddAdminUser: User is null");
                return new BadRequestResult();
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"UserRepo: AddAdminUser: Admin with email {user.Email} added");
            return new OkObjectResult(user);
        }

        public async Task<ActionResult<AuthResult>> UpdateUserAsync(UpdatePersonalDataRequest updatePersonalDataRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == updatePersonalDataRequest.Email);
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
            await _context.SaveChangesAsync();
            _logger.LogInformation($"UserRepo: UpdateUser: User with email {updatePersonalDataRequest.Email} updated");
            return new OkObjectResult(new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", ""));
        }

        public async Task<ActionResult<AuthResult>> DeleteUserAsync(AuthRequest authRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == authRequest.Email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: DeleteUser: User with email {authRequest.Email} not found");
                return new NotFoundResult();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"UserRepo: DeleteUser: User with email {authRequest.Email} deleted");
            return new OkObjectResult(new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", ""));
        }
    }
}
