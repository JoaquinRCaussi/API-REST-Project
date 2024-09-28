using Domain;
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
        List<User> users = _userLogic.GetUsers();
        return Ok(users);
    }

    [HttpGet]
    [Route("api/users/{userId}")]
    public IActionResult GetUser([FromRoute] Guid userId)
    {
        User user = _userLogic.GetUser(userId);
        return Ok(user);
    }

    [HttpDelete]
    [Route("api/users/{userId}")]
    public IActionResult DeleteUser([FromRoute] Guid userId)
    {
        User user = _userLogic.DeleteUser(userId);
        return Ok(user);
    }
}
