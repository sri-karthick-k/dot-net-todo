using TodoAPI.DTO;

namespace TodoAPI.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        
        //reference
        public required User user { get; set; }
    }
}
