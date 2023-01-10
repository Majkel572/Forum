using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
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

    public async Task<List<Post>> DeletePost(Post post){
        List<ForumWebAPI.Post> newPostList = await postRepo.DeletePost(post);
        return newPostList;
    }

    public async Task<List<Post>> GetPosts(){
        var postList = await postRepo.GetPosts();
        return postList;
    }
    #endregion
}
