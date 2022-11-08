using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
    [HttpPost("RegisterUser")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> RegisterUser([FromBody] RegisterUserDTO user){ 
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.RegisterUser(user);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(UserList);
    }

    [HttpPut]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> UpdateUser([FromBody] User p){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.UpdateUser(p);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        
        return Ok(UserList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlreadyRegisteredUserDTO>> GetUser([FromRoute] int id){
        Microsoft.AspNetCore.Mvc.ActionResult<ForumWebAPI.AlreadyRegisteredUserDTO> User;
        try {
            User = await userService.GetUser(id);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(User);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> DeleteUser([FromRoute] int id){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.DeleteUser(id);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(UserList);
    }

    [HttpGet("Users")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> GetUsers(){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.GetUsers();
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }        
        return Ok(UserList);
    }
    #endregion

    #region Login
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody]UserLoginDTO userLogin){
        string loggedInToken;
        try{
            loggedInToken = await userService.LoginUser(userLogin);
        } catch(ArgumentException e){
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return NotFound("User not founr");
        }
        return Ok(loggedInToken);
    }
    #endregion
    private Logs LogCreator(string message){
        Logs newLog = new Logs();
        newLog.Log = message;
        return newLog;
    }
}