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
        this._userLogic = userLogic;
    }
    
    [HttpPost]
    public IActionResult CreateAdmin(UserRequest user)
    {
        User userToCreate = user.ToArgs();
        User createdUser = _userLogic.CreateAdmin(userToCreate);
        UserResponse response = new UserResponse(createdUser);
        return Ok(response);
    }
}
