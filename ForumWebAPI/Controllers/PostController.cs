using System.Security.Claims;
using ForumWebAPI.UserDTOs;
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
    [Authorize]
    public async Task<ActionResult<string>> CreatePost([FromForm] string topic, [FromForm] string content, [FromForm] string role, [FromForm] string section, [FromForm] string email){
        var uploadedFile = Request.Form.Files;
        foreach (var file in uploadedFile)
        {
            string Filename = file.FileName;
        }
        var image = uploadedFile[0];
        PostDTO post = new PostDTO();
        post.Topic = topic;
        post.Content = content;
        post.Image = (FormFile)image;
        post.PostOwnerEmail = email;
        post.Section = section;
        
        try {
            if(role.Equals("default")){
                post.isDefaultPost = true;
            } else {
                post.isDefaultPost = false;
            }
            await userService.CreatePost(post);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok("Successfully created a new post.");
    }

    [HttpDelete("deleteDefaultpost")]
    [Authorize(Roles = "moderator, administrator")]
    public async Task<ActionResult<OkObjectResult>> DeletePost(Post post){ 
        var postList = new List<Post>();
        try {
            postList = await userService.DeletePost(post);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }

    [HttpGet("getdefaultposts")]
    [Authorize]
    public async Task<ActionResult<List<Post>>> GetPosts(){ 
        var postList = new List<Post>();
        try {
            postList = await userService.GetPosts();
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }
    #endregion

    private Logs LogCreator(string message){
        Logs newLog = new Logs();
        newLog.Log = message;
        return newLog;
    }

    private AlreadyRegisteredUserDTO GetCurrentUser(){
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if(identity != null){
            var userClaims = identity.Claims;
            var userHelp = new AlreadyRegisteredUserDTO();
            return new AlreadyRegisteredUserDTO{
                Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                Role = userHelp.RoleWriter(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value),
                Country = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Country)?.Value
            };
        }
        return null;
    }
}