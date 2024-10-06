using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    private readonly IHomeLogic _homeLogic;
    //PASAR RESPONSES

    public UserController(IUserLogic userLogic, IHomeLogic homeLogic)
    {
        _userLogic = userLogic;
        _homeLogic = homeLogic;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        List<User> users = _userLogic.GetUsers(); //Select(u => new UserResponse(u)).ToList();
        return Ok(users);
    }

    [HttpGet]
    [Route("{userId}")]
    public IActionResult GetUser([FromRoute] Guid userId)
    {
        User user = _userLogic.GetUser(userId);
        return Ok(user);
    }

    [HttpDelete]
    [Route("{userId}")]
    public IActionResult DeleteUser([FromRoute] Guid userId)
    {
        User user = _userLogic.DeleteUser(userId);
        return Ok(user);
    }
    
    [HttpGet]
    [Route("{userId}/homes")]
    public IActionResult GetUserHomes([FromRoute] Guid userId)
    {
        List<Home> homes = _homeLogic.GetHomesByUser(userId);
        return Ok(homes);
    }
}
