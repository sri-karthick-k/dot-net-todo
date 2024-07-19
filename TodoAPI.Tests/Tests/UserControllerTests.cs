using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoAPI.Controllers;
using TodoAPI.DTO;
using TodoAPI.Interfaces;

namespace TodoAPI.Tests.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUser> _mockUserRepository;
        UserController _userController;

        public UserControllerTests()
        {
            _mockUserRepository = new Mock<IUser>();
            //_userController = new UserController(_mockUserRepository.Object);
        }


        // Register function
        // Created!
        [Fact]
        public async Task UserController_Create_Task_ActionResult()
        {
            UserDTO userDTO = new UserDTO { Email = "sri1@mail.com", Name = "sri1", Password = "sri1" };

            _userController = new UserController(_mockUserRepository.Object);
            _mockUserRepository.Setup(r => r.Add(userDTO));
            var result = await _userController.Create(userDTO);
            Assert.NotNull(result);
            result.Should().BeOfType<CreatedResult>();
        }

        // NoContent!
        [Fact]
        public async Task UserController_Create_Task_ActionResult_NoContent()
        {
            UserDTO userDTO = new UserDTO();
            _userController = new UserController(_mockUserRepository.Object);
            var result = await _userController.Create(userDTO);
            result.Should().BeOfType<NoContentResult>();
        }

        [Theory]
        [InlineData("sri@mail.com", "sri")]
        public async Task UserController_Login_UserDTO_Ok(string email, string password)
        {
            UserDTO userDTO = new UserDTO() { Email = email, Password = password };
             
            _userController = new UserController(_mockUserRepository.Object);
            _mockUserRepository.Setup(r => r.Login(email, password))
                       .ReturnsAsync(userDTO);

            var result = await _userController.Login(userDTO);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Theory]
        [InlineData(null, "sri")]
        public async Task UserController_Login_UserDTO_BadRequest(string email, string password)
        {
            UserDTO userDTO = new UserDTO() { Email = email, Password = password };

            _userController = new UserController(_mockUserRepository.Object);
            _mockUserRepository.Setup(r => r.Login(email, password))
                       .ReturnsAsync(userDTO);

            var result = await _userController.Login(userDTO);

            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [InlineData("invalid@mail.com", "invalid")]
        public async Task UserController_Login_UserDTO_Unauthorized(string email, string password)
        {
            UserDTO userDTO = new UserDTO() { Email = email, Password = password };

            _userController = new UserController(_mockUserRepository.Object);
            _mockUserRepository.Setup(r => r.Login(email, password));

            var result = await _userController.Login(userDTO);

            result.Result.Should().BeOfType<UnauthorizedResult>();
        }

    }
}