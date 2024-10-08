using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/home-owner")]
public class HomeOwnerController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public HomeOwnerController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult CreateHomeOwner([FromBody] HomeOwnerRequest user)
    {
        User homeOwner = _userLogic.CreateHomeOwner(user.ToUser());
        return CreatedAtAction(nameof(CreateHomeOwner), new { id = homeOwner.Id }, homeOwner);
    }
}
