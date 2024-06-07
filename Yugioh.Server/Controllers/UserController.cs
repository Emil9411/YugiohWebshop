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
                _logger.LogError("UserContoller: GetUserByEmail: Error getting user by email");
                return NotFound("UserContoller: GetUserByEmail: Error getting user by email");
            }
            _logger.LogInformation("UserContoller: GetUserByEmail: User found by email");
            return Ok(user);
        }

        [HttpGet("getusers"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userRepoMultiple.GetUsersAsync();
            if (users == null)
            {
                _logger.LogError("UserContoller: GetAllUsers: Error getting all users");
                return NotFound("UserContoller: GetAllUsers: Error getting all users");
            }
            _logger.LogInformation("UserContoller: GetAllUsers: All users found");
            return Ok(users);
        }

        [HttpPost("addadminuser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAdminUser(RegistrationRequest registrationRequest)
        {
            var result = await _userRepoSingle.AddAdminUserAsync(registrationRequest);
            if (result == null)
            {
                _logger.LogError($"UserController: AddAdminUser: Error adding admin user {registrationRequest.Email}");
                return BadRequest($"UserController: AddAdminUser: Error adding admin user {registrationRequest.Email}");
            }
            _logger.LogInformation($"UserController: AddAdminUser: Admin user added {registrationRequest.Email}");
            return Ok(result);
        }

        [HttpPatch("updateuser"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<AuthResult>> UpdateUser([FromBody] UpdatePersonalDataRequest updatePersonalDataRequest)
        {
            var result = await _userRepoSingle.UpdateUserAsync(updatePersonalDataRequest);
            if (result == null)
            {
                _logger.LogError("UserController: UpdateUser: Error updating user");
                return BadRequest("UserController: UpdateUser: Error updating user");
            }
            _logger.LogInformation("UserController: UpdateUser: User updated");
            return Ok(result);
        }

        [HttpDelete("deleteuser"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<AuthResult>> DeleteUser(AuthRequest authRequest)
        {
            var result = await _userRepoSingle.DeleteUserAsync(authRequest);
            if (result == null)
            {
                _logger.LogError("UserController: DeleteUser: Error deleting user");
                return BadRequest("UserController: DeleteUser: Error deleting user");
            }
            _logger.LogInformation("UserController: DeleteUser: User deleted");
            return Ok(result);
        }

        [HttpDelete("deleteuseradmin/{email}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthResult>> DeleteUserAdmin(string email)
        {
            var result = await _userRepoSingle.DeleteUserAdminAsync(email);
            if (result == null)
            {
                _logger.LogError("UserController: DeleteUserAdmin: Error deleting user");
                return BadRequest("UserController: DeleteUserAdmin: Error deleting user");
            }
            _logger.LogInformation("UserController: DeleteUserAdmin: User deleted");
            return Ok(result);
        }
    }
}
