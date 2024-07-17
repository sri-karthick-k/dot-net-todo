using TodoAPI.DBContext;
using TodoAPI.Interfaces;
using TodoAPI.Models;
using Dapper;
using TodoAPI.DTO;

namespace TodoAPI.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DapperContext context;

        public UserRepository(DapperContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<UserDTO>> Get()
        {
            var sql = $@"SELECT [Id], [Email], [Name] FROM [Users]";
            using var connection = context.CreateConnection();
            return await connection.QueryAsync<UserDTO>(sql);
        }

        public async Task<UserDTO> Login(string email, string password)
        {
            var sql = $@"SELECT [Id], [Name], [Password] FROM [Users] WHERE [Email]=@email";
            using var connection = context.CreateConnection();
            var users = await connection.QueryAsync<UserDTO>(sql, new { Email = email });
            UserDTO user = users.FirstOrDefault();

            if (user != null && password != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return  user;
            }
            return null;
        }

        public async Task<UserDTO> Add(UserDTO user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var sql = $@"INSERT INTO Users ([Email], [Password], [Name]) VALUES (@Email, @Password, @Name)";

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(sql, user);
            return user;
        }

        public Task<UserDTO> Find(long Id)
        {
            throw new NotImplementedException();
        }



        public Task<UserDTO> Remove(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateDetails(string NewPassword, string NewName, long Id)
        {
            var sql = $@"UPDATE Users SET [PASSWORD] = @NewPassword, [NAME] = @NewName WHERE [Id] = @Id";

            var parameters = new { NewPassword, NewName, Id };

            using var connection = context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, parameters);
            return result;
        }
    }
}
