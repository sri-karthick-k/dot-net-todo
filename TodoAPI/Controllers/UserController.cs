using Microsoft.AspNetCore.Mvc;
using TodoAPI.DTO;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUser userRepository;

        public UserController(IUser userRepository)
        {
            this.userRepository = userRepository;
        }
        
        // Get all users (for debugging purposes)
        [HttpGet]
        [Route("api/users/getAll")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Index()
        {
            var users = await userRepository.Get();
            return Ok(users);
        }

        // register a user
        [HttpPost]
        public async Task<IActionResult> Create(UserDTO user)
        {
            if (user != null && user.Name != null && user.Email != null && user.Password != null)
            {
                await userRepository.Add(user);
                return Created();
            }
            return NoContent();
        }

        // user login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserDTO user)
        {
            string? Email = user.Email;
            string? Password = user.Password;
            if (Email != null && Password != null)
            {
                UserDTO resultantUser = await userRepository.Login(Email, Password);
                if(resultantUser != null)
                {
                    return Ok(resultantUser);
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        // user update
        [HttpPut("update_details")]
        public async Task<ActionResult<UserDTO>> UpdateDetails(UpdateUser updatedUser)
        {
            User user = updatedUser.user;

            string NewPassword = updatedUser.NewPassword;
            string NewName = updatedUser.NewName;
            if (user.Email != null && user.Password != null && NewPassword != null && NewName != null)
            {
                var resultantUser = await userRepository.Login(user.Email, user.Password);
                if (resultantUser != null)
                {
                    if (resultantUser != null)
                    {
                        var result = userRepository.UpdateDetails(NewPassword, NewName, user.Id);
                        Console.WriteLine(result);
                    }
                    return Ok(resultantUser);
                }
                return Unauthorized();
            }
            return BadRequest();
        }
    }
}
