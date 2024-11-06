using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/admins")]
[AuthenticationFilter]
[AuthorizationFilter("CanCreateAdmin")]
public sealed class AdminController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public AdminController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult CreateAdmin(AdminRequest admin)
    {
        User userToCreate = admin.ToArgs();
        User createdUser = _userLogic.CreateAdmin(userToCreate);
        var response = new AdminResponse
        {
            Name = createdUser.Name,
            LastName = createdUser.LastName,
            Email = createdUser.Email
        };
        return CreatedAtAction(nameof(CreateAdmin), new { id = createdUser.Id }, response);
    }

    [HttpDelete]
    [Route("{userId}")]
    public IActionResult DeleteUser([FromRoute] Guid userId)
    {
        User user = _userLogic.DeleteUser(userId);
        return Ok(user);
    }
}
