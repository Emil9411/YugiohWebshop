using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yugioh.Server.Controllers;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;
using Yugioh.Server.Services.AuthServices.Responses;
using Yugioh.Server.Services.AuthServices.Services;

namespace Yugioh.Server.Test
{
    internal class AuthControllerTest
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;

        public AuthControllerTest()
        {
            _authServiceMock = new Mock<IAuthService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
        }

        // Register tests

        [Test]
        public async Task Register_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AuthController(_authServiceMock.Object, _loggerMock.Object);
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await controller.Register(new RegistrationRequest("Email", "Username", "Password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Register_WhenRegistrationFails_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AuthController(_authServiceMock.Object, _loggerMock.Object);
            _authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(false, "Email", "Username", "Error"));

            // Act
            var result = await controller.Register(new RegistrationRequest("Email", "Username", "Password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Register_WhenRegistrationSucceeds_ReturnsCreatedAtAction()
        {
            // Arrange
            var controller = new AuthController(_authServiceMock.Object, _loggerMock.Object);
            _authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(true, "Email", "Username", "Error"));

            // Act
            var result = await controller.Register(new RegistrationRequest("Email", "Username", "Password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        }

        // Login tests

        [Test]
        public async Task Login_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AuthController(_authServiceMock.Object, _loggerMock.Object);
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await controller.Login(new AuthRequest("Email", "Password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Login_WhenLoginFails_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AuthController(_authServiceMock.Object, _loggerMock.Object);
            _authServiceMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(false, "Email", "Username", "Error"));

            // Act
            var result = await controller.Login(new AuthRequest("Email", "Password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Login_WhenLoginSucceeds_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            authServiceMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(true, "Email", "Username", "Token"));

            // Act
            var result = await controller.Login(new AuthRequest("Email", "Password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var authResponse = okResult?.Value as AuthResponse;
            Assert.That(authResponse?.Email, Is.EqualTo("Email"));
            Assert.That(authResponse.Username, Is.EqualTo("Username"));
        }

        // ChangePassword tests

        [Test]
        public async Task ChangePassword_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await controller.ChangePassword(new ChangePasswordRequest("Email", "OldPassword", "NewPassword"));

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task ChangePassword_WhenChangePasswordFails_ReturnsBadRequest()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            authServiceMock.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(false, "Email", "Username", "Error"));

            // Act
            var result = await controller.ChangePassword(new ChangePasswordRequest("Email", "OldPassword", "NewPassword"));

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task ChangePassword_WhenChangePasswordSucceeds_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = "Authorization=some-auth-token";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            authServiceMock.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(true, "Email", "Username", "Success"));

            // Act
            var result = await controller.ChangePassword(new ChangePasswordRequest("Email", "OldPassword", "NewPassword"));

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        // ChangeEmail tests

        [Test]
        public async Task ChangeEmail_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await controller.ChangeEmail(new UpdateEmailRequest("Email", "NewEmail"));

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task ChangeEmail_WhenChangeEmailFails_ReturnsBadRequest()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            authServiceMock.Setup(x => x.ChangeEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(false, "Email", "Username", "Error"));

            // Act
            var result = await controller.ChangeEmail(new UpdateEmailRequest("Email", "NewEmail"));

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task ChangeEmail_WhenChangeEmailSucceeds_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = "Authorization=some-auth-token";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            authServiceMock.Setup(x => x.ChangeEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult(true, "Email", "Username", "Success"));

            // Act
            var result = await controller.ChangeEmail(new UpdateEmailRequest("Email", "NewEmail"));

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        // WhoAmI tests

        [Test]
        public void WhoAmI_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var actionResult = controller.WhoAmI();

            // Assert
            Assert.That(actionResult, Is.InstanceOf<ActionResult<AuthResponse>>());
            var result = (ActionResult<AuthResponse>)actionResult;
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public void WhoAmI_WhenUserIsAuthenticated_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = "Authorization=some-auth-token";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            authServiceMock.Setup(x => x.Verify(It.IsAny<string>()))
                .Returns(new JwtSecurityToken(audience: "audience", issuer: "issuer", claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Email, "Email"),
                    new Claim(ClaimTypes.Name, "Username")
                }));

            // Act
            var actionResult = controller.WhoAmI();

            // Assert
            Assert.That(actionResult, Is.InstanceOf<ActionResult<AuthResponse>>());
            var result = (ActionResult<AuthResponse>)actionResult;
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        // Logout tests

        [Test]
        public void Logout_WhenUserIsNotAuthenticated_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var actionResult = controller.Logout();

            // Assert
            Assert.That(actionResult, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void Logout_WhenUserIsAuthenticated_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();

            var controller = new AuthController(authServiceMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = "Authorization=some-auth-token";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var actionResult = controller.Logout();

            // Assert
            Assert.That(actionResult, Is.InstanceOf<OkResult>());
        }
    }
}
