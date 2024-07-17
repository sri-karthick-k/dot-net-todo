using TodoAPI.DTO;
using TodoAPI.Models;

namespace TodoAPI.Interfaces
{
    public interface ITodo
    {
        Task<TodoItemDTO> Add(TodoItem todoItem);
        Task<IEnumerable<TodoItemDTO>> Get(long Id);
        Task<TodoItemDTO> GetTodo(long u_id, long Id);
        Task<int> Update(TodoItemDTO todoItemDTO);
        Task<int> Delete(long Id, long UId);
        Task<User> findUserById(long Id);
    }
}
