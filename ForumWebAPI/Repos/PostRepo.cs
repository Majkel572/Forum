using Microsoft.EntityFrameworkCore;

namespace ForumWebAPI.Repos;

public class PostRepo
{   
    private readonly DataContext dataContext;

    public PostRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<bool> AddPost(PostDTO post){
        byte[] imageData = null;
        using (MemoryStream ms = new MemoryStream())
        {
            await post.Image.CopyToAsync(ms);
            imageData = ms.ToArray();
        }
        Post p = new Post();
        p.Content = post.Content;
        p.UserEmail = post.PostOwnerEmail;
        p.Topic = post.Topic;
        p.isDefaultPost = post.isDefaultPost;
        p.ImageData = imageData;
        p.Section = post.Section;
        dataContext.Posts.Add(p);
        await dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Post>> UpdatePost(){
        await dataContext.SaveChangesAsync();
        return await dataContext.Posts.ToListAsync();
    }

    public async Task<Post> GetPost(int id){
        var Post = await dataContext.Posts.FindAsync(id);
        return Post;
    }

    public async Task<List<Post>> DeletePost(Post u){
        dataContext.Posts.Remove(u);
        await dataContext.SaveChangesAsync();
        return await dataContext.Posts.ToListAsync();
    }

    public async Task<List<Post>> GetPosts(){
        var PostList = await dataContext.Posts.ToListAsync();
        return PostList;
    }
}