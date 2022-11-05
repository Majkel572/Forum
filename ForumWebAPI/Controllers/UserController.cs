using Microsoft.AspNetCore.Mvc;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext dataContext;
    private UserService userService;

    public UserController(DataContext dataContext)
    {
        this.dataContext = dataContext;
        userService = new UserService(dataContext);
    }

    #region CRUD
    [HttpPost]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> RegisterUser([FromBody] RegisterUserDTO user){ 
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.RegisterUser(user);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        return Ok(UserList);
    }

    [HttpPut]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> UpdateUser([FromBody] User p){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.UpdateUser(p);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        
        return Ok(UserList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlreadyRegisteredUserDTO>> GetUser([FromRoute] int id){
        Microsoft.AspNetCore.Mvc.ActionResult<ForumWebAPI.AlreadyRegisteredUserDTO> User;
        try {
            User = await userService.GetUser(id);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        return Ok(User);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> DeleteUser([FromRoute] int id){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.DeleteUser(id);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        return Ok(UserList);
    }

    [HttpGet("Users")]
    public async Task<ActionResult<List<AlreadyRegisteredUserDTO>>> GetUsers(){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.AlreadyRegisteredUserDTO>> UserList;
        try {
            UserList = await userService.GetUsers();
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }        
        return Ok(UserList);
    }
    #endregion
}