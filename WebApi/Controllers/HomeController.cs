using Domain;
using IBusinessLogic;
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
        var user = (User)HttpContext.Items[0];
        Home homeToCreate = home.ToArgs();
        homeToCreate.HomeOwner = user.Id;
        Home createdHome = _homeLogic.CreateHome(homeToCreate);
        var response = new HomeResponse { Location = createdHome.Location, MemberCount = createdHome.MemberCount, HomeOwner = createdHome.HomeOwner, Latitude = createdHome.Latitude, Longitude = createdHome.Longitude };
        return CreatedAtAction(nameof(CreateHome), new { id = createdHome.Id }, response);
    }

    [HttpGet]
    public IActionResult GetHomes()
    {
        List<Home> homes = _homeLogic.GetHomes();
        var response = homes.Select(x => new HomeResponse { Location = x.Location, HomeOwner = x.HomeOwner, Devices = x.Devices, MemberCount = x.MemberCount, Latitude = x.Latitude, Longitude = x.Longitude }).ToList();
        return Ok(response);
    }

    [HttpGet]
    [Route("{homeId}")]
    public IActionResult GetHome(Guid homeId)
    {
        var home = _homeLogic.GetHome(homeId);
        var response = new HomeResponse { Location = home.Location, MemberCount = home.MemberCount, Devices = home.Devices, HomeOwner = home.HomeOwner, Latitude = home.Latitude, Longitude = home.Longitude };
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

    [HttpPut]
    [Route("{homeId}/members/{userId}")]
    public IActionResult UpdatePermissions(Guid homeId, Guid userId, [FromBody] PermissionRequest permissions)
    {
        var context = HttpContext;
        var user = (User)context.Items[0];
        var value = permissions.Value;
        if (value == "CanGetNotifications" && user.Id == userId)
        {
            var home = _homeLogic.UpdatePermissions(homeId, userId, permissions);
            return Ok(home);
        }

        var homeData = _homeLogic.GetHome(homeId);
        if (homeData.HomeOwner == user.Id)
        {
            var home = _homeLogic.UpdatePermissions(homeId, userId, permissions);
            return Ok(home);
        }
        else
        {
            return BadRequest(new { Message = "You are not the owner of this home" });
        }
    }

    [HttpPost]
    [AuthorizationFilter("CanAsociateDevices")]
    [Route("{homeId}/devices")]
    public IActionResult AddDeviceToHome(Guid homeId, [FromBody] HomeDeviceRequest deviceRequest)
    {
        var deviceId = deviceRequest.DeviceId.Value; ;
        var homeDevice = _homeLogic.AddDevice(homeId, deviceId);

        var response = new HomeDeviceResponse { HardwareId = homeDevice.HardwareId, Device = homeDevice.Device };

        return CreatedAtAction(nameof(AddDeviceToHome), new { id = homeDevice.HardwareId }, response);
    }

    [HttpGet]
    [Route("{homeId}/devices")]
    [AuthorizationFilter("CanListDevices")]
    public IActionResult GetHomeDevices(Guid homeId)
    {
        var devices = _homeLogic.GetHomeDevices(homeId);
        return Ok(devices);
    }

    //Justificacion en documentacion de por que esta en home controller
    [HttpPost]
    [Route("{homeId}/sensor/{hardwareId}/open")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationOpenSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "open";
        sensorRequest.Event = sensorEvent;
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorRequest);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationOpenSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/sensor/{hardwareId}/close")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationCloseSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "close";
        sensorRequest.Event = sensorEvent;
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorRequest);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationCloseSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/camera/{hardwareId}/person-detected")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationPersonDetectedCamera(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "person-detected";
        sensorRequest.Event = sensorEvent;
        var notifications = _homeLogic.CreateNotificationCamera(homeId, hardwareId, sensorRequest);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationPersonDetectedCamera), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/camera/{hardwareId}/movement-detected")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationMovementDetectedCamera(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "movement-detected";
        sensorRequest.Event = sensorEvent;
        var notifications = _homeLogic.CreateNotificationCamera(homeId, hardwareId, sensorRequest);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationMovementDetectedCamera), new { id = notification.Id }, notifications);
    }
    
    [HttpPut]
    [Route("{homeId}/devices/{hardwareId}")]
    [AuthorizationFilter("CanChangeDeviceName")]
    public IActionResult ChangeHomeDeviceName(Guid homeId, Guid hardwareId, [FromBody] string changeDeviceNameRequest)
    {
        var name = changeDeviceNameRequest;
        var homeDevice = _homeLogic.ChangeHomeDeviceName(homeId, hardwareId, name);
        return Ok(homeDevice);
    }
}
