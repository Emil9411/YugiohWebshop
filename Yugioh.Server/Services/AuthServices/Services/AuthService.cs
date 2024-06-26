﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;

namespace Yugioh.Server.Services.AuthServices.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                _logger.LogError($"AuthService: User with email {email} already exists");
                return new AuthResult(false, email, "", "User with this email already exists");
            }
            existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser != null)
            {
                _logger.LogError($"AuthService: User with username {username} already exists");
                return new AuthResult(false, "", username, "User with this username already exists");
            }

            var newUser = new User
            {
                Email = email,
                UserName = username
            };

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, role);
                _logger.LogInformation($"AuthService: User with email {email} created successfully");
                return new AuthResult(true, email, username, "");
            }
            else
            {
                _logger.LogError($"AuthService: Error creating user with email {email}");
                return FailedRegistration(result, email, username);
            }
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"AuthService: User with email {email} not found");
                return InvalidEmail(email);
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
            {
                _logger.LogError($"AuthService: Invalid password for user with email {email}");
                return InvalidPassword(email, user?.UserName ?? "");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.CreateToken(user, roles[0]);
            _logger.LogInformation($"AuthService: User with email {email} logged in successfully");
            return new AuthResult(true, email, user?.UserName ?? "", accessToken);
        }

        public async Task<AuthResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"AuthService: User with email {email} not found");
                return InvalidEmail(email);
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!validPassword)
            {
                _logger.LogError($"AuthService: Invalid password for user with email {email}");
                return InvalidPassword(email, user?.UserName ?? "");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation($"AuthService: Password for user with email {email} changed successfully");
                return new AuthResult(true, email, user?.UserName ?? "", "");
            }
            else
            {
                _logger.LogError($"AuthService: Error changing password for user with email {email}");
                return FailedRegistration(result, email, user?.UserName ?? "");
            }
        }

        public async Task<AuthResult> ChangeEmailAsync(string email, string newEmail)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"AuthService: User with email {email} not found");
                return InvalidEmail(email);
            }

            user.Email = newEmail;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation($"AuthService: Email for user with email {email} changed successfully");
                return new AuthResult(true, newEmail, user?.UserName ?? "", "");
            }
            else
            {
                _logger.LogError($"AuthService: Error changing email for user with email {email}");
                return FailedRegistration(result, email, user?.UserName ?? "");
            }
        }

        public JwtSecurityToken Verify(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuerSigningKey = _configuration.GetSection("IssuerSigningKey").Value;
            var key = !string.IsNullOrEmpty(issuerSigningKey) ? Encoding.UTF8.GetBytes(issuerSigningKey) : throw new ArgumentNullException("IssuerSigningKey is null or empty");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
        {
            var autResult = new AuthResult(false, email, username, "");
            foreach (var error in result.Errors)
            {
                autResult.ErrorMessages.Add(error.Code, error.Description);
            }
            return autResult;
        }

        private static AuthResult InvalidUsername(string username)
        {
            var result = new AuthResult(false, "", username, "");
            result.ErrorMessages.Add("Bad credentials", "Invalid username");
            return result;
        }

        private static AuthResult InvalidPassword(string email, string userName)
        {
            var result = new AuthResult(false, email, userName, "");
            result.ErrorMessages.Add("Bad credentials", "Invalid password");
            return result;
        }

        private static AuthResult InvalidEmail(string email)
        {
            var result = new AuthResult(false, email, "", "");
            result.ErrorMessages.Add("Bad credentials", "Invalid email");
            return result;
        }
    }
}
