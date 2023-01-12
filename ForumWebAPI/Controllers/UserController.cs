using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ForumWebAPI.Services;
using ForumWebAPI.UserDTOs;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext dataContext;
    private AuthAuthService authAuthService;
    private UserService userService;
    private readonly ILogger<UserController> logger;
    public UserController(DataContext dataContext, ILogger<UserController> logger, IConfiguration iconfig)
    {
        this.dataContext = dataContext;
        this.authAuthService = new AuthAuthService(dataContext, iconfig);
        this.userService = new UserService(dataContext, iconfig);
        this.logger = logger;
    }

    #region CRUD
    [HttpPost("register")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> RegisterUser([FromBody] RegisterUserDTO user){ 
        try {
            await userService.RegisterUser(user);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest("User exists or there was a problem creating one.");
        }
        return Ok("Account successfuly created, waiting for validation.");
    }

    [HttpPost("validate")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> ValidateUser(int validationCode, string email){ 
        try {
            await userService.ValidateUser(validationCode, email);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest("Bad code");
        }
        return Ok("Successfully validated account.");
    }

    [HttpPut("update")]
    [Authorize(Roles = "default, administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> UpdateUser([FromBody] RegisterUserDTO user){
        var currentUser = GetCurrentUser(); //autoryzacja
        try {
            await userService.UpdateUser(user);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        
        return Ok("Successfully updated information.");
    }
    #region administrators
    [HttpGet("get")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<AlreadyRegisteredUserDTO>> GetUser([FromRoute] string email){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<AlreadyRegisteredUserDTO> User;
        try {
            User = await userService.GetUser(email);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(User);
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> DeleteUser([FromRoute] string email){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.DeleteUser(email);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }
        return Ok(UserList);
    }

    [HttpGet("getall")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> GetUsers(){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<AlreadyRegisteredUserDTO>> UserList;
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

    [HttpGet("pageusers")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> PageUsers([FromBody]int[] itemsToShowAndPages){
        var currentUser = GetCurrentUser(); //autoryzacja
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.PageUsers(itemsToShowAndPages[0], itemsToShowAndPages[1]);
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }        
        return Ok(UserList);
    }

    [HttpGet("getadminlist")]
    [Authorize(Roles = "administrator")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> GetAdminList(){
        var currentUser = GetCurrentUser(); //autoryzacja
        List<AlreadyRegisteredUserDTO> adminList;
        try {
            adminList = await userService.ListAdministrators();
        } catch(ArgumentException e){ 
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return BadRequest();
        }        
        return Ok(adminList);
    }

    [HttpPost("checkAuth")]
    [Authorize]
    public async Task<ActionResult<string>> CheckUserPerm(){
        var currentUser = GetCurrentUser();
        if(currentUser.Role.Equals(Roles.ADMINISTRATOR)){
            return "admin";
        } else if(currentUser.Role.Equals(Roles.MODERATOR)){
            return "mod";
        } else if(currentUser.Role.Equals(Roles.OWNER)){
            return "owner";
        } else {
            return "default";
        }
    }

    [HttpPost("whoiam")]
    [Authorize]
    public async Task<ActionResult<string>> WhoIAm(){
        var currentUser = GetCurrentUser();
        return "username: " + currentUser.Username + " Role: " + currentUser.RoleReader();
    }

    [HttpPost("signPost")]
    [Authorize]
    public async Task<ActionResult<string>> SignPost(){
        var currentUser = GetCurrentUser();
        return currentUser.Email + " " + currentUser.RoleReader() + " " + currentUser.Username;
    }

    [HttpPost("rolegetter")]
    [Authorize]
    public async Task<ActionResult<string>> RoleGettere(){
        var currentUser = GetCurrentUser();
        return currentUser.RoleReader();
    }
    #endregion
    #endregion

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
                Country = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Country)?.Value,
                Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
            };
        }
        return null;
    }

    private Logs LogCreator(string message){
        Logs newLog = new Logs();
        newLog.Log = message;
        return newLog;
    }
}