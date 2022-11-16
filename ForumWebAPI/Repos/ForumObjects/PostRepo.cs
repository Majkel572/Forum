using Microsoft.EntityFrameworkCore;

namespace ForumWebAPI;

public class PostRepo
{   
    private readonly DataContext dataContext;

    public PostRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<List<Post>> AddPost(Post p){
        dataContext.Posts.Add(p);
        await dataContext.SaveChangesAsync();
        return await dataContext.Posts.ToListAsync();
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