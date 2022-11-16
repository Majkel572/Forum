namespace ForumWebAPI;

public class Post{
    public int Id { get; set; }
    public string content { get; set; }
    public User PostOwner { get; set; } 
}