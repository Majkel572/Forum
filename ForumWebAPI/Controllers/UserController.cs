using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext dataContext;
    private UserService ps;

    public UserController(DataContext dataContext)
    {
        this.dataContext = dataContext;
        ps = new UserService(dataContext);
    }

    #region CRUD
    [HttpPost]
    public async Task<ActionResult<List<User>>> AddUser([FromBody] User p){ 
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.User>> UserList;
        try {
            UserList = await ps.AddUser(p);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        return Ok(UserList);
    }

    [HttpPut]
    public async Task<ActionResult<List<User>>> UpdateUser([FromBody] User p){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.User>> UserList;
        try {
            UserList = await ps.UpdateUser(p);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        
        return Ok(UserList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser([FromRoute] int id){
        Microsoft.AspNetCore.Mvc.ActionResult<ForumWebAPI.User> User;
        try {
            User = await ps.GetUser(id);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        return Ok(User);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<User>>> DeleteUser([FromRoute] int id){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.User>> UserList;
        try {
            UserList = await ps.DeleteUser(id);
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }
        return Ok(UserList);
    }

    [HttpGet("Users")]
    public async Task<ActionResult<List<User>>> GetUsers(){
        Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.List<ForumWebAPI.User>> UserList;
        try {
            UserList = await ps.GetUsers();
        } catch(ArgumentException e){ 
            return BadRequest();
            //dodać logger
        }        
        return Ok(UserList);
    }
    #endregion
}