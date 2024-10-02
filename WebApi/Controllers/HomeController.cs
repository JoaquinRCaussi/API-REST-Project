using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

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
        var response = new HomeResponse { Location = createdHome.Location, MemberCount = createdHome.MemberCount, Devices = createdHome.Devices, HomeOwner = createdHome.HomeOwner };
        return Ok(response);
    }

    [HttpGet]
    [Route("{homeId}/members")]
    public IActionResult GetHomeMembers(Guid homeId)
    {
        var users = _homeLogic.GetHomeMembers(homeId);
        var response = users.Select(x => new GetHomeMembersResponse
        {
            Email = x.Email,
            Name = x.Name,
        }
            ).ToList();
        return Ok(response);
    }

    [HttpPut]
    [Route("{homeId}")]
    public IActionResult AddMemberToHome(Guid homeId, [FromBody] Guid userId)
    {
        var home = _homeLogic.AddMember(homeId, userId);
        return Ok(home);
    }
}
