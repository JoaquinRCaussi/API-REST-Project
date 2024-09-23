using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AdminController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    
    public AdminController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }
    
    [HttpPost]
    public IActionResult CreateAdmin(User user)
    {
        var createdUser = _userLogic.CreateAdmin(user);
        return Ok(createdUser);
    }
}
