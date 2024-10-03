using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/homes")]
[AuthenticationFilter]
public class HomeController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;
    private readonly IMemberSettingLogic _memberSettingLogic;

    public HomeController(IHomeLogic homeLogic, IMemberSettingLogic memberSettingLogic)
    {
        _homeLogic = homeLogic;
        _memberSettingLogic = memberSettingLogic;
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
    public IActionResult GetHomes()
    {
        List<Home> homes = _homeLogic.GetHomes();
        var response = homes.Select(x => new HomeResponse { Location = x.Location, HomeOwner = x.HomeOwner, Devices = x.Devices, MemberCount = x.MemberCount }).ToList();
        return Ok(response);
    }

    [HttpGet]
    [Route("{userId}")]
    public IActionResult GetHomeByUser(Guid userId)
    {
        var homes = _homeLogic.GetHomesByUser(userId);
        var response = homes.Select(x => new HomeResponse { Location = x.Location, HomeOwner = x.HomeOwner, Devices = x.Devices, MemberCount = x.MemberCount }).ToList();
        return Ok(response);
    }

    [HttpGet]
    [Route("{homeId}")]
    public IActionResult GetHome(Guid homeId)
    {
        var home = _homeLogic.GetHome(homeId);
        var response = new HomeResponse { Location = home.Location, MemberCount = home.MemberCount, Devices = home.Devices, HomeOwner = home.HomeOwner };
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
    [AuthorizationFilter("CanAddMembers")]
    public IActionResult AddMemberToHome(Guid homeId, [FromBody] AddMemberRequest addMemberRequest)
    {
        var userId = Guid.Parse(addMemberRequest.UserId!);
        
        var home = _homeLogic.AddMember(homeId, userId);
        var memberSetting = _memberSettingLogic.CreateMemberSetting(homeId, userId);

        var response = new AddMemberResponse { Home = home, MemberSetting = memberSetting };
        
        return Ok(response);
    }
    
    //Members porque cuando haga {homeid}/members traigo los usuarios, selecciono uno de ahi y le cambio los permisos en {homeid}/members/{userid}
    [HttpPut]
    [Route("{homeId}/members/{userId}")]
    public IActionResult UpdatePermissions(Guid homeId, Guid userId, [FromBody] PermissionRequest permissions)
    {
        var home = _homeLogic.UpdatePermissions(homeId, userId, permissions);
        return Ok(home);
    }
    
}
