using TodoAPI.DTO;
using TodoAPI.Models;

namespace TodoAPI.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<UserDTO>> Get();
        Task<UserDTO> Login(string email, string password);
        Task<UserDTO> Find(long Id);
        Task<UserDTO> Add(UserDTO user);
        Task<int> UpdateDetails(string NewPassword, string NewName, long Id);
        Task<UserDTO> Remove(UserDTO user);


    }
}
