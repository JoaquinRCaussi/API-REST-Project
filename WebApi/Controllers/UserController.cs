using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
[AuthenticationFilter]
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
        List<User> users = _userLogic.GetUsers();
        
        List<GetUserResponse> response = users.Select(x => new GetUserResponse
        {
            Name = x.Name,
            LastName = x.LastName,
            CreatedAt = x.CreatedAt,
            Email = x.Email,
            Role = x.Role
        }).ToList();
        
        return Ok(response);
    }

    [HttpGet]
    [Route("{userId}")]
    public IActionResult GetUser([FromRoute] Guid userId)
    {
        User user = _userLogic.GetUser(userId);
        GetUserResponse response = new GetUserResponse
        {
            Name = user.Name,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
            Role = user.Role
        };
        return Ok(response);
    }

    [HttpDelete]
    [Route("{userId}")]
    public IActionResult DeleteUser([FromRoute] Guid userId)
    {
        User user = _userLogic.DeleteUser(userId);
        
        GetUserResponse response = new GetUserResponse
        {
            Name = user.Name,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
            Role = user.Role
        };
        
        return Ok(response);
    }

    [HttpGet]
    [Route("{userId}/homes")]
    public IActionResult GetUserHomes([FromRoute] Guid userId)
    {
        List<Home> homes = _homeLogic.GetHomesByUser(userId);
        return Ok(homes);
    }

    [HttpGet]
    [Route("{userId}/notifications")]
    public IActionResult GetUserNotifications([FromRoute] Guid userId)
    {
        List<Notification> notifications = _userLogic.GetNotifications(userId);
        return Ok(notifications);
    }
}
