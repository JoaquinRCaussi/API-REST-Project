using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/homes")]
public class HomeController: ControllerBase
{
    private readonly IHomeLogic _homeLogic;
    
    public HomeController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }
    
    [HttpPost]
    public IActionResult CreateHome(HomeRequest home)
    {
        Home homeToCreate = home.ToArgs();
        Home createdHome = _homeLogic.CreateHome(homeToCreate);
        var response = new HomeResponse(createdHome);
        return Ok(response);
    }
}
