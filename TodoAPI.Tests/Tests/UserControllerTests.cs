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
            var result = await _userController.Create(userDTO);
            Assert.NotNull(result);
            result.Should().BeOfType<CreatedResult>();
            _mockUserRepository.Verify(r => r.Add(It.IsAny<UserDTO>()), Times.Once);
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
        [InlineData("sri@mail.com", "sri", "sri", "PASS")]
        [InlineData("sri1@mail.com", "sri1", "sri", "FAIL")]
        [InlineData(null, "sri", null, "FAIL")]
        public async Task UserController_Login_Task_UserDTO(string email, string password, string name, string type)
        {
            UserDTO userDTO = new UserDTO() { Email = email, Password = password, Name = name };
             
            _userController = new UserController(_mockUserRepository.Object);
            //await _userController.Create(userDTO);
            if (type == "PASS")
            {
                _mockUserRepository.Setup(r => r.Login(email, password))
                       .ReturnsAsync(userDTO);
            }

            var result = await _userController.Login(userDTO);
            if (email == null || password == null || name == null)
            {
                result.Result.Should().BeOfType<BadRequestResult>();
            }
            else if(type == "PASS")
            {
                
                result.Result.Should().BeOfType<OkObjectResult>();
            }
            else
            {
                result.Result.Should().BeOfType<UnauthorizedResult>();
            }
        }
    }
}