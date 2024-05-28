﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Contracts;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;
using Yugioh.Server.Services.AuthServices.Responses;
using Yugioh.Server.Services.AuthServices.Services;

namespace Yugioh.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AuthController: Register: Model state is invalid");
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(request.Email, request.Username, request.Password, "User");

            if (!result.Success)
            {
                AddErrors(result);
                _logger.LogError("AuthController: Register: Registration failed");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("AuthController: Register: Registration successful");
            return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.Username));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AuthController: Login: Model state is invalid");
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                AddErrors(result);
                _logger.LogError("AuthController: Login: Login failed");
                return BadRequest(ModelState);
            }

            Response.Cookies.Append("Authorization", result.Token, new CookieOptions
            {
                HttpOnly = true
            });

            _logger.LogInformation("AuthController: Login: Login successful");
            return Ok(new AuthResponse(result.Email, result.Username));
        }

        [HttpGet("whoami"), Authorize(Roles = "User, Admin")]
        public ActionResult<AuthResponse> WhoAmI()
        {
            var cookie = Request.Cookies["Authorization"];

            if (cookie == null)
            {
                _logger.LogError("AuthController: WhoAmI: No cookie found");
                return BadRequest("AuthController: WhoAmI: No cookie found");
            }

            var result = _authService.Verify(cookie);

            if (result != null)
            {
                var claims = result.Claims.ToList();
                var email = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? "";
                var username = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value ?? "";

                _logger.LogInformation("AuthController: WhoAmI: User found");
                return Ok(new AuthResponse(email, username));
            }

            _logger.LogError("AuthController: WhoAmI: User not found");
            return BadRequest("AuthController: WhoAmI: User not found");

        }

        [HttpPost("logout"), Authorize(Roles = "User, Admin")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("Authorization");

            _logger.LogInformation("AuthController: Logout: Logout successful");
            return Ok();
        }

        private void AddErrors(AuthResult result)
        {
            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }
    }
}