using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ForumWebAPI.Services;
using ForumWebAPI.UserDTOs;

namespace ForumWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthAuthController : ControllerBase
{
    private readonly DataContext dataContext;
    private AuthAuthService authAuthService;
    private readonly ILogger<AuthAuthController> logger;
    public AuthAuthController(DataContext dataContext, ILogger<AuthAuthController> logger, IConfiguration iconfig)
    {
        this.dataContext = dataContext;
        authAuthService = new AuthAuthService(dataContext, iconfig);
        this.logger = logger;
    }

    #region Login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO userLogin)
    {
        string loggedInToken;
        try
        {
            loggedInToken = await authAuthService.LoginUser(userLogin);
        }
        catch (ArgumentException e)
        {
            dataContext.Logs.Add(LogCreator(e.ToString()));
            await dataContext.SaveChangesAsync();
            logger.LogError(new ArgumentException(), "Errored error");
            return NotFound("User not found");
        }
        return Ok(loggedInToken);
    }
    #endregion
    private Logs LogCreator(string message)
    {
        Logs newLog = new Logs();
        newLog.Log = message;
        return newLog;
    }
}