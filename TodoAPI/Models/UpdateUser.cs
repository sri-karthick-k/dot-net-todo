namespace TodoAPI.Models
{
    public class UpdateUser
    {
        public User user {  get; set; }

        public string NewPassword { get; set; }

        public string NewName { get; set; }
    }
}
