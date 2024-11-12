using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
[AuthenticationFilter]
public class UserController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    private readonly IHomeLogic _homeLogic;

    public UserController(IUserLogic userLogic, IHomeLogic homeLogic)
    {
        _userLogic = userLogic;
        _homeLogic = homeLogic;
    }

    [HttpGet]
    [AuthorizationFilter("CanManageUsers")]
    public IActionResult GetUsers(
    [FromQuery] string? role,
    [FromQuery] string? fullName,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        List<User> users = _userLogic.GetUsersFiltered(role, fullName);

        var totalResults = users.Count;

        var paginatedUsers = users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = paginatedUsers.Select(x => new GetUserResponse
        {
            Name = x.Name,
            LastName = x.LastName,
            CreatedAt = x.CreatedAt,
            Email = x.Email,
            Role = x.Role
        }).ToList();

        return Ok(new
        {
            TotalResults = totalResults,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Users = response
        });
    }

    [HttpGet]
    [Route("{userId}")]
    [AuthorizationFilter("CanManageUsers")]
    public IActionResult GetUser([FromRoute] Guid userId)
    {
        User user = _userLogic.GetUser(userId);
        var response = new GetUserResponse
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
    [AuthorizationFilter("CanManageUsers")]
    public IActionResult DeleteUser([FromRoute] Guid userId)
    {
        User user = _userLogic.DeleteUser(userId);

        var response = new GetUserResponse
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
