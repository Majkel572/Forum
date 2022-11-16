using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ForumWebAPI;

public class User{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Country { get; set; }
    public DateTime BirthDate { get; set; }
    [Unique]
    public string Email { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public Roles Role { get; set; }        
    public List<Post> Posts { get; set; }
}