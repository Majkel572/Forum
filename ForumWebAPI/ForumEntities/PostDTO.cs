
namespace ForumWebAPI;

public class PostDTO
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public string PostOwnerEmail { get; set; }
    public string Topic { get; set; }
    public IFormFile Image { get; set; }
}