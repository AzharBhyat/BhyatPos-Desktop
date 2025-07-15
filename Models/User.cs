namespace BhyatPos.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Hash only!
        public string Role { get; set; }
    }

}
