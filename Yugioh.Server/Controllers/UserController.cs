using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;
using Yugioh.Server.Services.UserRepository;

namespace Yugioh.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepoSingle _userRepoSingle;
        private readonly IUserRepoMultiple _userRepoMultiple;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepoSingle userRepoSingle, IUserRepoMultiple userRepoMultiple, ILogger<UserController> logger)
        {
            _userRepoSingle = userRepoSingle;
            _userRepoMultiple = userRepoMultiple;
            _logger = logger;
        }

        [HttpGet("getuserbyemail/{email}"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail(string email)
        {
            var user = await _userRepoSingle.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError("Controller: Error getting user by email");
                return NotFound("Controller: Error getting user by email");
            }
            _logger.LogInformation("Controller: User found by email");
            return Ok(user);
        }

        [HttpGet("getusers"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepoMultiple.GetUsersAsync();
            if (users == null)
            {
                _logger.LogError("Controller: Error getting all users");
                return NotFound("Controller: Error getting all users");
            }
            _logger.LogInformation("Controller: All users found");
            return Ok(users);
        }

        [HttpPost("addadminuser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAdminUser(RegistrationRequest registrationRequest)
        {
            var result = await _userRepoSingle.AddAdminUserAsync(registrationRequest);
            if (result == null)
            {
                _logger.LogError($"Controller: Error adding admin user {registrationRequest.Email}");
                return BadRequest($"Controller: Error adding admin user {registrationRequest.Email}");
            }
            _logger.LogInformation($"Controller: Admin user added {registrationRequest.Email}");
            return Ok(result);
        }

        [HttpPatch("updateuser"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<AuthResult>> UpdateUser([FromBody] UpdatePersonalDataRequest updatePersonalDataRequest)
        {
            var result = await _userRepoSingle.UpdateUserAsync(updatePersonalDataRequest);
            if (result == null)
            {
                _logger.LogError("Controller: Error updating user");
                return BadRequest("Controller: Error updating user");
            }
            _logger.LogInformation("Controller: User updated");
            return Ok(result);
        }

        [HttpDelete("deleteuser"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<AuthResult>> DeleteUser(AuthRequest authRequest)
        {
            var result = await _userRepoSingle.DeleteUserAsync(authRequest);
            if (result == null)
            {
                _logger.LogError("Controller: Error deleting user");
                return BadRequest("Controller: Error deleting user");
            }
            _logger.LogInformation("Controller: User deleted");
            return Ok(result);
        }

        [HttpDelete("deleteuseradmin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthResult>> DeleteUserAdmin(string email)
        {
            var result = await _userRepoSingle.DeleteUserAdminAsync(email);
            if (result == null)
            {
                _logger.LogError("Controller: Error deleting user");
                return BadRequest("Controller: Error deleting user");
            }
            _logger.LogInformation("Controller: User deleted");
            return Ok(result);
        }
    }
}
