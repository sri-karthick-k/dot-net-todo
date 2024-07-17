using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.DTO;
using TodoAPI.Interfaces;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodo todoRepository;

        public TodoItemsController(ITodo todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        private static TodoItemDTO ItemsToDTO(TodoItem todoItem) =>
           new TodoItemDTO
           {
               UserId = todoItem.Id,
               Name = todoItem.Name,
               IsComplete = todoItem.IsComplete,
           };
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) {
            return new TodoItemDTO
            {
                UserId = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };
        }


        // GET: api/TodoItems
        [HttpGet]
        public async Task<IEnumerable<TodoItemDTO>> GetTodoItems(long UId)
        {
            var todos = await todoRepository.Get(UId);
            return todos;
        }

        // GET: api/TodoItems/5?id=1
        [HttpGet("{id}")]
        public async Task<TodoItemDTO> GetTodoItem(long id, [FromQuery] long UId)
        {
            var todo = await todoRepository.GetTodo(UId, id);
            return todo;
        }


        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, [FromBody] TodoItemDTO todoItemDTO)
        {
            todoItemDTO.Id = id;
            var result = await todoRepository.Update(todoItemDTO);
            if(result == 1)
            {
                return Ok(todoItemDTO);
            }
            return NotFound();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
        {
            // fetch user by id
            var user = await todoRepository.findUserById(todoDTO.UserId);
            if(user == null)
            {
                return NotFound();
            }

            var todoItem = new TodoItem
            {
                IsComplete = todoDTO.IsComplete,
                Name = todoDTO.Name,
                user =  user,
            };

            var todoItemDTO = await todoRepository.Add(todoItem);

            return Ok(todoItemDTO);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id, [FromQuery] long UId)
        {
            var result = await todoRepository.Delete(id, UId);
            if(result == 1)
            {
                return Ok();
            }
            return NotFound();
        }

        private bool TodoItemExists(long id)
        {
            //return _context.TodoItems.Any(e => e.Id == id);
            return false;
        }
    }
}
