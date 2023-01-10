using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly DataContext dataContext;
    private PostService userService;
    private readonly ILogger<UserController> logger;
    public PostController(DataContext dataContext, ILogger<UserController> logger)
    {
        this.dataContext = dataContext;
        userService = new PostService(dataContext);
        this.logger = logger;
    }

    #region CRUD
    [HttpPost("newpost")]
    [Authorize(Roles = "default, moderator, administrator")]
    public async Task<ActionResult<OkObjectResult>> CreatePost([FromBody] PostDTO post){ 
        try {
            await userService.CreatePost(post);
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok("Successfully created a new post.");
    }

    [HttpDelete("deletepost")]
    [Authorize(Roles = "moderator, administrator")]
    public async Task<ActionResult<OkObjectResult>> DeletePost(Post post){ 
        var postList = new List<Post>();
        try {
            postList = await userService.DeletePost(post);
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }

    [HttpGet("getposts")]
    [Authorize(Roles = "moderator, administrator")]
    public async Task<ActionResult<List<Post>>> GetPosts(){ 
        var postList = new List<Post>();
        try {
            postList = await userService.GetPosts();
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }
    #endregion
}