
namespace ForumWebAPI;

public class PostDTO
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public string PostOwnerEmail { get; set; }
    public string Topic { get; set; }
    public FormFile Image { get; set; }
    public bool isDefaultPost { get; set; }
    public string Section { get; set; }
    public string Username { get; set; }
}