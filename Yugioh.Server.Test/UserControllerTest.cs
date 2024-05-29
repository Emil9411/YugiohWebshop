using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Yugioh.Server.Controllers;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Requests;
using Yugioh.Server.Services.UserRepository;

namespace Yugioh.Server.Test
{
    internal class UserControllerTest
    {
        private readonly Mock<IUserRepoSingle> _userRepoSingleMock;
        private readonly Mock<IUserRepoMultiple> _userRepoMultipleMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _userRepoSingleMock = new Mock<IUserRepoSingle>();
            _userRepoMultipleMock = new Mock<IUserRepoMultiple>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userRepoSingleMock.Object, _userRepoMultipleMock.Object, _loggerMock.Object);
        }

        readonly User user = new User
        {
            Id = "1",
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            City = "Test City",
            Country = "Test Country",
            Address = "Test Address",
            PostalCode = "12345",
            PhoneNumber = "123456789"
        };

        // GetUserByEmail Tests

        [Test]
        public async Task GetUserByEmail_ReturnsUser_WhenUserExists()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserByEmail(user.Email);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetUserByEmail_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _userController.GetUserByEmail(user.Email);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        // GetUserById Tests

        [Test]
        public async Task GetUserById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserById(user.Id);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _userController.GetUserById(user.Id);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        // GetAllUsers Tests

        [Test]
        public async Task GetAllUsers_ReturnsUsers_WhenUsersExist()
        {
            // Arrange
            _userRepoMultipleMock.Setup(x => x.GetUsersAsync()).ReturnsAsync(new List<User> { user });

            // Act
            var result = await _userController.GetAllUsers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllUsers_ReturnsNotFound_WhenUsersDoNotExist()
        {
            // Arrange
            _userRepoMultipleMock.Setup(x => x.GetUsersAsync()).ReturnsAsync((IEnumerable<User>)null);

            // Act
            var result = await _userController.GetAllUsers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        // AddAdminUser Tests

        [Test]
        public async Task AddAdminUser_ReturnsUser_WhenUserIsNotNull()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.AddAdminUserAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userController.AddAdminUser(user);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task AddAdminUser_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.AddAdminUserAsync(It.IsAny<User>())).ReturnsAsync((User)null);

            // Act
            var result = await _userController.AddAdminUser(null);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        // UpdateUser Tests

        [Test]
        public async Task UpdateUser_ReturnsAuthResult_WhenUserExists()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.UpdateUserAsync(It.IsAny<UpdatePersonalDataRequest>())).ReturnsAsync(new AuthResult(true, user.Email, user.Email, "asd"));

            // Act
            var result = await _userController.UpdateUser(new UpdatePersonalDataRequest(user.Email, user.FirstName, user.LastName, user.Country, user.City, user.Address, user.PostalCode, user.PhoneNumber));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateUser_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.UpdateUserAsync(It.IsAny<UpdatePersonalDataRequest>())).ReturnsAsync((AuthResult)null);

            // Act
            var result = await _userController.UpdateUser(new UpdatePersonalDataRequest(user.Email, user.FirstName, user.LastName, user.Country, user.City, user.Address, user.PostalCode, user.PhoneNumber));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        // DeleteUser Tests

        [Test]
        public async Task DeleteUser_ReturnsAuthResult_WhenUserExists()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.DeleteUserAsync(It.IsAny<AuthRequest>())).ReturnsAsync(new AuthResult(true, user.Email, user.Email, "asd"));

            // Act
            var result = await _userController.DeleteUser(new AuthRequest(user.Email, "password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteUser_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.DeleteUserAsync(It.IsAny<AuthRequest>())).ReturnsAsync((AuthResult)null);

            // Act
            var result = await _userController.DeleteUser(new AuthRequest(user.Email, "password"));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        // DeleteUserAdmin Tests

        [Test]
        public async Task DeleteUserAdmin_ReturnsAuthResult_WhenUserExists()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.DeleteUserAdminAsync(It.IsAny<string>())).ReturnsAsync(new AuthResult(true, user.Email, user.Email, "asd"));

            // Act
            var result = await _userController.DeleteUserAdmin(user.Email);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteUserAdmin_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoSingleMock.Setup(x => x.DeleteUserAdminAsync(It.IsAny<string>())).ReturnsAsync((AuthResult)null);

            // Act
            var result = await _userController.DeleteUserAdmin(user.Email);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
