namespace ForumWebAPI;

public class Post
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public string PostOwnerEmail { get; set; }
    public string Topic { get; set; }
    public byte[] ImageData { get; set; }
}