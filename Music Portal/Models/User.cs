using System.ComponentModel.DataAnnotations;

namespace Music_Portal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Access_Level { get; set; }
    }
}
