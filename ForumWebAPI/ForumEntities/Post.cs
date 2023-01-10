namespace ForumWebAPI;

public class Post
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public string UserEmail { get; set; }
    public string Topic { get; set; }
    public byte[] ImageData { get; set; }
    public bool isDefaultPost { get; set; }
    public string? Section { get; set; }
}