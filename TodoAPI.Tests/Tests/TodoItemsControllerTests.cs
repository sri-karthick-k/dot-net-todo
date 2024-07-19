using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.DTO;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Tests.Tests
{
    public class TodoItemsControllerTests
    {
        private readonly Mock<ITodo> _mockRepository;
        TodoItemsController _controller;

        public TodoItemsControllerTests()
        {
            _mockRepository = new Mock<ITodo>();
        }

        // GET /api/TodoItems (user_id)
        [Fact]
        public async Task TodoItemsController_GetTodoItems_Returns_ListTodoItemsDTO()
        {
            _controller = new TodoItemsController(_mockRepository.Object);
            var mockTodos = new List<TodoItemDTO>()
            {
                new TodoItemDTO{ Id = 1, Name = "abc", IsComplete = true },
                new TodoItemDTO{Id = 2, Name = "def", IsComplete = false }
            };
            _mockRepository.Setup(r => r.Get(1)).ReturnsAsync(mockTodos);
            var result = await _controller.GetTodoItems((long)1);
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<IEnumerable<TodoItemDTO>>>();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        // GET /api/TodoItems (user_id, todo_id)
        [Fact]
        public async Task TodoItemsController_GetTodoItems_Returns_NoContent()
        {
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.Get(1));
            var result = await _controller.GetTodoItems(1);

            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<IEnumerable<TodoItemDTO>>>();
            result.Result.Should().BeOfType<NoContentResult>();
        }

        // GET /api/TodoItems (user_id, todo_id)
        [Fact]
        public async Task TodoItemsController_GetTodoItem_Returns_Ok()
        {
            var mockTodo = new TodoItemDTO { Id = 1, Name = "abc", IsComplete = true };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.GetTodo(1, 1)).ReturnsAsync(mockTodo);
            var result = await _controller.GetTodoItem(1, 1);

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
            var mockResult = result.Result as OkObjectResult;
            var mockValue = mockResult.Value as TodoItemDTO;

            mockValue.Should().NotBeNull();
            mockValue.Should().BeEquivalentTo(mockTodo);
        }

        // Getting okobjectresult but the value is null how? - because the value is stored in the Result object not in the result. So to access it, we need to store the Result as OkObjectResult since ActionResult does not directly expose the Value


        [Theory]
        [InlineData("todo1", true, 1)]
        [InlineData("todo2", true, 1)]
        [InlineData("todo3", false, 1)]
        [InlineData("todo4", false, 2)]
        public async Task TodoItemsController_PostTodoItem_Returns_TodoItemDTO_Ok(string Name, bool IsComplete, long UserId)
        {
            TodoItemDTO todoItemDTO = new TodoItemDTO { Name = Name, IsComplete = IsComplete, UserId = UserId };
            User user = new User { 
                Id = UserId,
                Email = "user1@mail.com",
                Password = "user1",
                Name = "user1"
            };
            TodoItem todoItem = new TodoItem { Name = Name, IsComplete = IsComplete, user = user };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.findUserById(todoItemDTO.UserId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.Add(todoItem)).ReturnsAsync(todoItemDTO);

            var result = await _controller.PostTodoItem(todoItemDTO);
            result.Should().NotBeNull();
            var mockResult = (OkObjectResult) result.Result;
            mockResult.Should().BeOfType<OkObjectResult>();
        }

        [Theory]
        [InlineData("todo1", true, 1)]
        [InlineData("todo4", false, 2)]
        public async Task TodoItemsController_PostTodoItem_Returns_TodoItemDTO_NotFound(string Name, bool IsComplete, long UserId)
        {
            TodoItemDTO todoItemDTO = new TodoItemDTO { Name = Name, IsComplete = IsComplete, UserId = UserId };
            User user = null;
            TodoItem todoItem = new TodoItem { Name = Name, IsComplete = IsComplete, user = user };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.findUserById(todoItemDTO.UserId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.Add(todoItem)).ReturnsAsync(todoItemDTO);

            var result = await _controller.PostTodoItem(todoItemDTO);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData(1, "todo1", "newTodo1", false, true, 1)]
        [InlineData(2, "todo2", "newTodo2", false, true, 1)]
        [InlineData(3, "todo3", "newTodo3", true, false, 1)]
        [InlineData(4, "todo4", "newTodo4", false, false, 2)]
        public async Task TodoItemsController_PutTodoItem_Returns_TodoItemDTO_Ok(long id, string Name, string NewName, bool IsComplete, bool NewIsComplete, long UserId)
        {
            int success = 1;
            TodoItemDTO todoItemDTO = new TodoItemDTO { Id = id, Name = Name, IsComplete = IsComplete, UserId = UserId };
            User user = new User
            {
                Id = UserId,
                Email = "user1@mail.com",
                Password = "user1",
                Name = "user1"
            };
            TodoItem todoItem = new TodoItem { Name = Name, IsComplete = IsComplete, user = user };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.findUserById(todoItemDTO.UserId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.Add(todoItem)).ReturnsAsync(todoItemDTO);
            todoItemDTO.Name = NewName;
            todoItemDTO.IsComplete = NewIsComplete;
            _mockRepository.Setup(r => r.Update(todoItemDTO)).ReturnsAsync(success);

            var result = await _controller.PutTodoItem(id, todoItemDTO);

            result.Should().NotBeNull();
            var mockResult = (OkObjectResult) result;
            mockResult.Should().BeOfType<OkObjectResult>();
        }

        [Theory]
        [InlineData(1, "todo1", "newTodo1", false, true, 1)]
        [InlineData(2, "todo2", "newTodo2", false, true, 1)]
        [InlineData(3, "todo3", "newTodo3", true, false, 1)]
        [InlineData(4, "todo4", "newTodo4", false, false, 2)]
        public async Task TodoItemsController_PutTodoItem_Returns_TodoItemDTO_NotFound(long id, string Name, string NewName, bool IsComplete, bool NewIsComplete, long UserId)
        {
            int failure = 0;
            TodoItemDTO todoItemDTO = new TodoItemDTO { Id = id, Name = Name, IsComplete = IsComplete, UserId = UserId };
            User user = new User
            {
                Id = UserId,
                Email = "user1@mail.com",
                Password = "user1",
                Name = "user1"
            };
            TodoItem todoItem = new TodoItem { Name = Name, IsComplete = IsComplete, user = user };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.findUserById(todoItemDTO.UserId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.Add(todoItem)).ReturnsAsync(todoItemDTO);
            todoItemDTO.Id += 10;
            todoItemDTO.Name = NewName;
            todoItemDTO.IsComplete = NewIsComplete;
            todoItemDTO.UserId += 10;
            _mockRepository.Setup(r => r.Update(todoItemDTO)).ReturnsAsync(failure);

            var result = await _controller.PutTodoItem(id, todoItemDTO);

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData(1, "todo1", true, 1)]
        [InlineData(2, "todo2", true, 1)]
        [InlineData(3, "todo3", false, 1)]
        [InlineData(4, "todo4", false, 2)]
        public async Task TodoItemsController_DeleteTodoItem_Returns_TodoItemDTO_Ok(long id, string Name, bool IsComplete, long UserId)
        {
            int success = 1;
            TodoItemDTO todoItemDTO = new TodoItemDTO { Id = id, Name = Name, IsComplete = IsComplete, UserId = UserId };
            User user = new User
            {
                Id = UserId,
                Email = "user1@mail.com",
                Password = "user1",
                Name = "user1"
            };

            TodoItem todoItem = new TodoItem { Name = Name, IsComplete = IsComplete, user = user };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.findUserById(todoItemDTO.UserId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.Add(todoItem)).ReturnsAsync(todoItemDTO);
            _mockRepository.Setup(r => r.Delete(id, UserId)).ReturnsAsync(success);

            var result = await _controller.DeleteTodoItem(id, UserId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }

        [Theory]
        [InlineData(1, "todo1", true, 1)]
        [InlineData(2, "todo2", true, 1)]
        [InlineData(3, "todo3", false, 1)]
        [InlineData(4, "todo4", false, 2)]
        public async Task TodoItemsController_DeleteTodoItem_Returns_TodoItemDTO_NotFound(long id, string Name, bool IsComplete, long UserId)
        {
            int failure = 0;
            TodoItemDTO todoItemDTO = new TodoItemDTO { Id = id, Name = Name, IsComplete = IsComplete, UserId = UserId };
            User user = new User
            {
                Id = UserId,
                Email = "user1@mail.com",
                Password = "user1",
                Name = "user1"
            };

            TodoItem todoItem = new TodoItem { Name = Name, IsComplete = IsComplete, user = user };
            _controller = new TodoItemsController(_mockRepository.Object);
            _mockRepository.Setup(r => r.findUserById(todoItemDTO.UserId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.Add(todoItem)).ReturnsAsync(todoItemDTO);
            _mockRepository.Setup(r => r.Delete(id, UserId)).ReturnsAsync(failure);

            var result = await _controller.DeleteTodoItem(id, UserId);

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }


    }
}
