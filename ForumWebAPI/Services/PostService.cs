using ForumWebAPI.Repos;

namespace ForumWebAPI;

public class PostService
{   
    private readonly DataContext dataContext;
    private PostRepo postRepo;
    public PostService(DataContext dataContext)
    {
        this.dataContext = dataContext;
        postRepo = new PostRepo(dataContext);
    }

    #region CRUD
    public async Task<bool> CreatePost(PostDTO post){ 
        await postRepo.AddPost(post);
        return true;
    }
    public async Task<bool> UpdatePost(PostDTO post, int id){ 
        await postRepo.UpdatePost(post, id);
        return true;
    }

    public async Task<List<Post>> DeletePost(int id){
        List<ForumWebAPI.Post> newPostList = await postRepo.DeletePost(id);
        return newPostList;
    }

    public async Task<List<Post>> GetDefPosts(){
        var postList = await postRepo.GetDefPosts();
        return postList;
    }

    public async Task<List<Post>> GetAdmPosts(){
        var postList = await postRepo.GetAdmPosts();
        return postList;
    }
    public async Task<Post> GetPostById(int id){
        var post = await postRepo.GetPostById(id);
        return post;
    }
    #endregion
}
