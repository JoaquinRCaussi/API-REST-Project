using LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    
    public UserController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }
    
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userLogic.GetUsers();
        return Ok(users);
    }
    
}
