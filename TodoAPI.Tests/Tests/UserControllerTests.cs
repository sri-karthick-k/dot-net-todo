using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.DTO;
using TodoAPI.Interfaces;

namespace TodoAPI.Tests.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUser> _mockUserRepository;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserRepository = new Mock<IUser>();
            _userController = new UserController(_mockUserRepository.Object);
        }


        // Register function
        // Created!
        [Fact]
        public async Task UserController_Create_Task_ActionResult()
        {
            UserDTO userDTO = new UserDTO { Email = "sri1@mail.com", Name = "sri1", Password = "sri1" };

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
            //await _userController.Create(userDTO);
            if(type == "PASS")
            {
                _mockUserRepository.Setup(r => r.Login(email, password))
                       .ReturnsAsync(new UserDTO { Email = email, Password = password, Name = name });
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