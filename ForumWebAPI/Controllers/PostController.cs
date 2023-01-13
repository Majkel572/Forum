using System.Security.Claims;
using ForumWebAPI.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ForumWebAPI.RegexCheckers;

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
    public async Task<ActionResult<string>> CreatePost([FromForm] string topic, [FromForm] string content, [FromForm] string role, [FromForm] string section, [FromForm] string email, [FromForm] string username){
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
        post.Username = username;
        RegexChecker rg = new RegexChecker();
        rg.CheckPost(post);
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

    [HttpPut("editpost")]
    [Authorize]
    public async Task<ActionResult<string>> UpdatePost([FromForm] string topic, [FromForm] string content, [FromForm] string role, [FromForm] string section, [FromForm] string email, [FromForm] string username, [FromForm] int postId){
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
        post.Username = username;
        RegexChecker rg = new RegexChecker();
        rg.CheckPost(post);
        try {
            if(role.Equals("default")){
                post.isDefaultPost = true;
            } else {
                post.isDefaultPost = false;
            }
            await userService.UpdatePost(post, postId);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok("Successfully created a new post.");
    }


    [HttpDelete("deletepost")]
    [Authorize(Roles = "default, moderator, administrator, owner")]
    public async Task<ActionResult<List<Post>>> DeletePost(string id){ 
        var currentUser = GetCurrentUser(); //autoryzacja


        int ide = int.Parse(id);
        var postList = new List<Post>();
        try {
            postList = await userService.DeletePost(ide);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }

    [HttpGet("getdefaultposts")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Post>>> GetDefPosts(){ 
        var postList = new List<Post>();
        try {
            postList = await userService.GetDefPosts();
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }

    [HttpGet("getstaffposts")]
    [Authorize(Roles = "moderator, administrator, owner")]
    public async Task<ActionResult<List<Post>>> GetAdmPosts(){ 
        var currentUser = GetCurrentUser(); //autoryzacja
        var postList = new List<Post>();
        try {
            postList = await userService.GetAdmPosts();
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(postList);
    }

    [HttpGet("getpostbyid")]
    public async Task<ActionResult<Post>> GetPostById([FromQuery] int id){ 
        Post post = new Post();
        try {
            post = await userService.GetPostById(id);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        PostBase64 pb64 = new PostBase64();
        pb64.Content = post.Content;
        pb64.isDefaultPost = post.isDefaultPost;
        pb64.PostId = post.PostId;
        pb64.UserEmail = post.UserEmail;
        pb64.Section = post.Section;
        pb64.Topic = post.Topic;
        pb64.Username = post.Username;
        pb64.ImageData = System.Convert.ToBase64String(post.ImageData);
        return Ok(pb64);
    }

    // [HttpGet("getpostbyid")]
    // [AllowAnonymous]
    // public async Task<ActionResult> GetPostById([FromQuery] int id){ 
    //     PostDTO post = new PostDTO();
    //     try {
    //         post = await userService.GetPostById(id);
    //     } catch(ArgumentException e){ 
    //         dataContext.Logs.Add(LogCreator(e.ToString()));
    //         await dataContext.SaveChangesAsync();
    //         logger.LogError(new ArgumentException(), "Errored error");
    //         return BadRequest();
    //     }
    //     PostNoImage pni = new PostNoImage();
    //     pni.Content = post.Content;
    //     pni.isDefaultPost = post.isDefaultPost;
    //     pni.PostId = post.PostId;
    //     pni.PostOwnerEmail = post.PostOwnerEmail;
    //     pni.Section = post.Section;
    //     pni.Topic = post.Topic;
    //     pni.Username = post.Username;
    //     return Ok(pni);
    // }
    // [HttpGet("getpostbyidimage")]
    // [AllowAnonymous]
    // public async Task<ActionResult> GetPostByIdImage([FromQuery] int id){ 
    //     PostDTO post = new PostDTO();
    //     try {
    //         post = await userService.GetPostById(id);
    //     } catch(ArgumentException e){ 
    //         dataContext.Logs.Add(LogCreator(e.ToString()));
    //         await dataContext.SaveChangesAsync();
    //         logger.LogError(new ArgumentException(), "Errored error");
    //         return BadRequest();
    //     }
    //     return Ok(File(post.Image.OpenReadStream(), post.Image.ContentType, post.Image.FileName));
    // }
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