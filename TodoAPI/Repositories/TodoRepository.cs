using Dapper;
using TodoAPI.DBContext;
using TodoAPI.DTO;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Repositories
{
    public class TodoRepository : ITodo
    {
        private readonly DapperContext context;

        public TodoRepository(DapperContext context)
        {
            this.context = context;
        }

        public async Task<TodoItemDTO> Add(TodoItem todoItem)
        {
            var sql = $@"INSERT INTO TodoItems ([Name], [IsComplete], [u_id]) VALUES(@Name, @IsComplete, @userId)";
            using var connection = context.CreateConnection();
            await connection.ExecuteScalarAsync<long>(sql, new
            {
                todoItem.Name,
                todoItem.IsComplete,
                userId = todoItem.user.Id,
            });

            Console.WriteLine("User Id: "+todoItem.user.Id);
            TodoItemDTO todoItemDTO = new TodoItemDTO()
            {
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
                UserId = todoItem.user.Id,
            };
            return todoItemDTO;
        }

        public async Task<int> Delete(long Id, long UId)
        {
            var sql = $@"DELETE FROM TodoItems WHERE [Id] = @Id AND [u_id] = @UId";
            using var connection = context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id, UId });
            return result;
        }

        public async Task<IEnumerable<TodoItemDTO>> Get(long UId)
        {
            var sql = $@"SELECT * FROM TodoItems WHERE [u_id] = @UId";
            using var connection = context.CreateConnection();
            var todoItems = await connection.QueryAsync<TodoItemDTO>(sql, new{ UId });
            return todoItems;
        }

        public async Task<TodoItemDTO> GetTodo(long UId, long Id)
        {
            var sql = $@"SELECT * FROM TodoItems WHERE [u_id] = @UId and [Id] = @Id";
            using var connection = context.CreateConnection();
            var todoItems = await connection.QueryAsync<TodoItemDTO>(sql, new { UId, Id });
            var todoItem = todoItems.FirstOrDefault();
            return todoItem;
        }

        public async Task<int> Update(TodoItemDTO todoItemDTO)
        {
            var sql = $@"UPDATE TodoItems SET [Name] = @Name, [IsComplete] = @IsComplete WHERE [u_id] = @UId and [Id] = @Id";
            using var connection = context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { todoItemDTO.Name, todoItemDTO.IsComplete, UId = todoItemDTO.UserId, todoItemDTO.Id });
            return result;
        }

        public async Task<User> findUserById(long UId) 
        {
            var sql = $@"SELECT * FROM Users WHERE [Id] = @UId";
            using var connection = context.CreateConnection();
            var user = await connection.QueryAsync<User>(sql, new { UId });
            return user.FirstOrDefault();
        }

    }
}
