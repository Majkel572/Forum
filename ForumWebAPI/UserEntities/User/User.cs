
namespace ForumWebAPI;

public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Country { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public Roles Role { get; set; }
    public List<Post> UserPosts { get; set; }
    public bool isValidated { get; set; }
    public string validationCode { get; set; }
    public string lastLogin { get; set; }
    public int loginCounter { get; set; }

}