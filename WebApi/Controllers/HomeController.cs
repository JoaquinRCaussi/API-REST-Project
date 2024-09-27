using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/homes")]
public class HomeController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;

    public HomeController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }

    [HttpPost]
    public IActionResult CreateHome([FromBody] HomeRequest home)
    {
        Home homeToCreate = home.ToArgs();
        Home createdHome = _homeLogic.CreateHome(homeToCreate);
        var response = new HomeResponse{ Location = createdHome.Location, MemberCount = createdHome.MemberCount, Devices = createdHome.Devices, HomeOwner = createdHome.HomeOwner };
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetHomes()
    {
        List<Home> homes = _homeLogic.GetHomes();
        var response = homes.Select(x => new HomeResponse{ Location = x.Location, HomeOwner = x.HomeOwner, Devices = x.Devices, MemberCount = x.MemberCount}).ToList();
        return Ok(response);
    }

    [HttpGet]
    [Route("homes/{userId}")]
    public IActionResult GetHomeByUser(Guid userId)
    {
        var homes = _homeLogic.GetHomesByUser(userId);
        var response = homes.Select(x => new HomeResponse{ Location = x.Location, HomeOwner = x.HomeOwner, Devices = x.Devices, MemberCount = x.MemberCount}).ToList();
        return Ok(response);
    }
    
    [HttpGet]
    [Route("homes/{homeId}")]
    public IActionResult GetHome(Guid homeId)
    {
        var home = _homeLogic.GetHome(homeId);
        var response = new HomeResponse{ Location = home.Location, MemberCount = home.MemberCount, Devices = home.Devices, HomeOwner = home.HomeOwner };
        return Ok(response);
    }
    
    [HttpGet]
    [Route("homes/{homeId}/members")]
    public IActionResult GetHomeMembers(Guid homeId)
    {
        var  users = _homeLogic.GetHomeMembers(homeId);
        var response = users.Select(x => new GetHomeMembersResponse
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
            }
            ).ToList();
        return Ok(response);
    }
}
