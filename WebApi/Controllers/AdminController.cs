using Domain;
using DTOS;
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
    public IActionResult CreateAdmin(AdminRequest admin)
    {
        User userToCreate = admin.ToArgs();
        User createdUser = _userLogic.CreateAdmin(userToCreate);
        var response = new AdminResponse(createdUser);
        return Ok(response);
    }
}
