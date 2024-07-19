using TodoAPI.DTO;
using TodoAPI.Models;

namespace TodoAPI.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<UserDTO>> Get();
        Task<UserDTO> Login(string email, string password);
        Task<UserDTO> Add(UserDTO user);
        Task<int> UpdateDetails(string NewPassword, string NewName, long Id);


    }
}
