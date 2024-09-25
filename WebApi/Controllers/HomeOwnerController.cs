using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/home-owner")]
public class HomeOwnerController: ControllerBase
{
    private readonly IUserLogic _userLogic;

    public HomeOwnerController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult CreateHomeOwner(User user)
    {
        User homeOwner = _userLogic.CreateHomeOwner(user);
        return Ok(homeOwner);
    }
    
}
