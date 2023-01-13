namespace ForumWebAPI;

public class PostBase64
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public string UserEmail { get; set; }
    public string Topic { get; set; }
    public string ImageData { get; set; }
    public bool isDefaultPost { get; set; }
    public string? Section { get; set; }
    public string Username { get; set; }
}