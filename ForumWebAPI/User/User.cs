namespace ForumWebAPI
{
    public class User{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Username { set{ Username = value; }}
        public string HashedPassword { set{ HashedPassword = value; } }
        public Roles Role { get; set; }
    }
}