using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext dataContext;
    private UserService userService;
    private readonly ILogger<UserController> logger;
    public UserController(DataContext dataContext, ILogger<UserController> logger, IConfiguration iconfig)
    {
        this.dataContext = dataContext;
        userService = new UserService(dataContext, iconfig);
        this.logger = logger;
    }

    #region CRUD
    [HttpPost("register")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> RegisterUser([FromBody] RegisterUserDTO user){ 
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.RegisterUser(user);
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok("Successfully created account.");
    }

    [HttpPut("update")]
    [Authorize(Roles = "default, administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> UpdateUser([FromBody] User p){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.UpdateUser(p);
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        
        return Ok("Successfully updated information.");
    }
    #region administrators
    [HttpGet("get")]
    //[Authorize(Roles = "administrator")]
    public async Task<ActionResult<AlreadyRegisteredUserDTO>> GetUser([FromRoute] int id){
        //var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<ForumWebAPI.AlreadyRegisteredUserDTO> User;
        try {
            User = await userService.GetUser(id);
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(User);
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> DeleteUser([FromRoute] int id){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.DeleteUser(id);
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(UserList);
    }

    [HttpGet("getall")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> GetUsers(){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.GetUsers();
        } catch(ArgumentException e){ 
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }        
        return Ok(UserList);
    }
    #endregion
    #endregion

    #region Login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody]UserLoginDTO userLogin){
        string loggedInToken;
        try{
            loggedInToken = await userService.LoginUser(userLogin);
        } catch(ArgumentException e){
            //dataContext.Logs.Add(LogCreator(e.ToString()));
            //await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return NotFound("User not found");
        }
        return Ok(loggedInToken);
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