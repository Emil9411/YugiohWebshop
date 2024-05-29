﻿using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public UserRepo(UsersContext context, ILogger<UserRepo> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            _logger.LogInformation($"UserRepo: GetUserByEmail: User with email {user.Email} found");
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: GetUserByEmail: User with email {email} not found");
                return null;
            }
            _logger.LogInformation($"UserRepo: GetUserByEmail: User with email {email} found");
            return user;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: GetUserById: User with id {id} not found");
                return null;
            }
            _logger.LogInformation($"UserRepo: GetUserById: User with id {id} found");
            return user;
        }

        public async Task<IEnumerable<User>?> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
            {
                _logger.LogWarning("UserRepo: GetUsers: No users found");
                return null;
            }
            _logger.LogInformation("UserRepo: GetUsers: Users found");
            return users;
        }

        public async Task<AuthResult?> AddAdminUserAsync(RegistrationRequest registrationRequest)
        {
            var user = new User
            {
                UserName = registrationRequest.Username,
                Email = registrationRequest.Email,
            };
            var result = await _userManager.CreateAsync(user, registrationRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                _logger.LogInformation($"UserRepo: AddAdminUser: Admin user with email {registrationRequest.Email} added");
                return new AuthResult(true, user.Email, user.UserName, "");
            }
            _logger.LogWarning($"UserRepo: AddAdminUser: Admin user with email {registrationRequest.Email} not added");
            return new AuthResult(false, "", "", "User not added");
        }

        public async Task<AuthResult?> UpdateUserAsync(UpdatePersonalDataRequest updatePersonalDataRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == updatePersonalDataRequest.Email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: UpdateUser: User with email {updatePersonalDataRequest.Email} not found");
                return null;
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
            return new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", "");
        }

        public async Task<AuthResult?> DeleteUserAsync(AuthRequest authRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == authRequest.Email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: DeleteUser: User with email {authRequest.Email} not found");
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"UserRepo: DeleteUser: User with email {authRequest.Email} deleted");
            return new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", "");
        }

        public async Task<AuthResult?> DeleteUserAdminAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning($"UserRepo: DeleteUserAdmin: User with email {email} not found");
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"UserRepo: DeleteUserAdmin: User with email {email} deleted");
            return new AuthResult(true, user?.Email ?? "", user?.UserName ?? "", "");
        }
    }
}
