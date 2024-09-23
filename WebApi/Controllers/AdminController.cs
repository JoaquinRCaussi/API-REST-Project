using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOS;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AdminController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    
    public AdminController(IUserLogic userLogic)
    {
        this._userLogic = userLogic;
    }
    
    [HttpPost]
    public IActionResult CreateAdmin(User user)
    {
        User createdUser = _userLogic.CreateAdmin(user);
        UserResponse response = new UserResponse(createdUser);
        return Ok(response);
    }
}
