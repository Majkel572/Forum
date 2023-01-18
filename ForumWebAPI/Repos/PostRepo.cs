using Microsoft.EntityFrameworkCore;

namespace ForumWebAPI.Repos;

public class PostRepo
{
    private readonly DataContext dataContext;

    public PostRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<bool> AddPost(PostDTO post)
    {
        byte[] imageData = null;
        Console.WriteLine(post.Image);
        using (MemoryStream ms = new MemoryStream())
        {
            await post.Image.CopyToAsync(ms);
            imageData = ms.ToArray();
        }
        byte[] jpegSig = new byte[] { 0xff, 0xd8, 0xff };
        if (imageData[0] == jpegSig[0] && imageData[1] == jpegSig[1] && imageData[2] == jpegSig[2]){
            Post p = new Post();
            p.Content = post.Content;
            p.UserEmail = post.PostOwnerEmail;
            p.Topic = post.Topic;
            p.isDefaultPost = post.isDefaultPost;
            p.ImageData = imageData;
            p.Section = post.Section;
            p.Username = post.Username;
            dataContext.Posts.Add(p);
            await dataContext.SaveChangesAsync();
            return true;
        }
        throw new ArgumentException("Bad image");
    }
    public async Task<bool> UpdatePost(PostDTO post, int id)
    {
        byte[] imageData = null;
        using (MemoryStream ms = new MemoryStream())
        {
            await post.Image.CopyToAsync(ms);
            imageData = ms.ToArray();
        }
        var p = dataContext.Posts.FirstOrDefault(x => x.PostId == id);
        if(p is null){
            return false;
        }
        p.Content = post.Content;
        p.Topic = post.Topic;
        p.ImageData = imageData;
        dataContext.Update(p);
        await dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Post>> UpdatePost()
    {
        await dataContext.SaveChangesAsync();
        return await dataContext.Posts.ToListAsync();
    }

    public async Task<Post> GetPost(int id)
    {
        var Post = await dataContext.Posts.FindAsync(id);
        return Post;
    }

    public async Task<List<Post>> DeletePost(int id)
    {
        var post = await dataContext.Posts.FindAsync(id);
        if(post is null){
            return await dataContext.Posts.ToListAsync();
        }
        dataContext.Posts.Remove(post);
        await dataContext.SaveChangesAsync();
        return await dataContext.Posts.ToListAsync();
    }

    public async Task<List<Post>> GetDefPosts()
    {
        var PostList = await dataContext.Posts.ToListAsync();
        List<Post> list = new List<Post>();
        foreach (Post p in PostList)
        {
            if (p.Section.Equals("default"))
            {
                Post postek = new Post();
                postek.Content = p.Content;
                postek.Topic = p.Topic;
                postek.Username = p.Username;
                postek.PostId = p.PostId;
                list.Add(postek);
            }
        }
        return list;
    }

    public async Task<List<Post>> GetAdmPosts()
    {
        var PostList = await dataContext.Posts.ToListAsync();
        List<Post> list = new List<Post>();
        foreach (Post p in PostList)
        {
            if (p.Section.Equals("staff"))
            {
                Post postek = new Post();
                postek.Content = p.Content;
                postek.Topic = p.Topic;
                postek.Username = p.Username;
                postek.PostId = p.PostId;
                list.Add(postek);
            }
        }
        return list;
    }

    public async Task<Post> GetPostById(int id)
    {
        var post = await dataContext.Posts.FindAsync(id);
        // PostDTO p = new PostDTO();
        // p.Content = post.Content;
        // p.isDefaultPost = post.isDefaultPost;
        // p.PostId = post.PostId;
        // p.PostOwnerEmail = post.UserEmail;
        // p.Section = post.Section;
        // p.Topic = post.Topic;
        // p.Username = post.Username;
        // string contentType = "image/jpeg";

        // var formFile = new FormFile(new MemoryStream(post.ImageData), 0, post.ImageData.Length, "image.jpg", contentType);

        return post;
    }
}