using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using WebApi.Models.In;
using WebApi.Models.Out;

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
        var response = new HomeResponse { Name = createdHome.Name, Location = createdHome.Location, MemberCount = createdHome.MemberCount, HomeOwner = createdHome.HomeOwner, Latitude = createdHome.Latitude, Longitude = createdHome.Longitude };
        return CreatedAtAction(nameof(CreateHome), new { id = createdHome.Id }, response);
    }


    [HttpGet]
    public IActionResult GetHomes()
    {
        var user = (User)HttpContext.Items[0];
        List<Home> homes = _homeLogic.GetHomesByUser(user.Id);
        return Ok(homes);
    }

    [HttpGet]
    [Route("{homeId}")]
    public IActionResult GetHome(Guid homeId)
    {
        var home = _homeLogic.GetHome(homeId);
        var response = new HomeResponse { Name = home.Name, Location = home.Location, MemberCount = home.MemberCount, Devices = home.Devices, HomeOwner = home.HomeOwner, Latitude = home.Latitude, Longitude = home.Longitude };
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
    [Route("{homeId}/members")]
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
            var home = _homeLogic.UpdatePermissions(homeId, userId, permissions.Value, permissions.Enable);
            return Ok(home);
        }

        var homeData = _homeLogic.GetHome(homeId);
        if (homeData.HomeOwner == user.Id)
        {
            var home = _homeLogic.UpdatePermissions(homeId, userId, permissions.Value, permissions.Enable);
            return Ok(home);
        }
        else
        {
            return BadRequest(new { Message = "You are not the owner of this home" });
        }
    }

    [HttpGet]
    [Route("{homeId}/members/{userId}")]
    public IActionResult GetMemberSetting(Guid homeId, Guid userId)
    {
        var memberSetting = _memberSettingLogic.GetMemberSetting(homeId, userId);

        var response = new GetMemberSettingResponse()
        {
            PermissionsValue = memberSetting.Permissions.Select(p => p.Value).ToList()
        };

        return Ok(response);
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
    public IActionResult GetHomeDevices(Guid homeId, Guid? roomId = null)
    {
        var devices = _homeLogic.GetHomeDevices(homeId, roomId);
        return Ok(devices);
    }

    [HttpPost]
    [Route("{homeId}/windowSensor/{hardwareId}/open")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationOpenSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "open";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationOpenSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/windowSensor/{hardwareId}/close")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationCloseSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "close";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationCloseSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/smartLamp/{hardwareId}/on")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationTurnOnSmartLamp(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "on";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationTurnOnSmartLamp), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/smartLamp/{hardwareId}/off")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationTurnOffSmartLamp(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "off";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationTurnOffSmartLamp), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/movementSensor/{hardwareId}/open")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationOpenMovementSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "open";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationOpenMovementSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/movementSensor/{hardwareId}/close")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationCloseMovementSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "close";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationCloseMovementSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/movementSensor/{hardwareId}/movement-detected")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationMovementDetectedSensor(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "movement-detected";
        var notifications = _homeLogic.CreateNotificationSensor(homeId, hardwareId, sensorEvent);
        var notification = notifications.First();
        return CreatedAtAction(nameof(CreateNotificationMovementDetectedSensor), new { id = notification.Id }, notifications);
    }

    [HttpPost]
    [Route("{homeId}/camera/{hardwareId}/person-detected")]
    [AuthorizationFilter]
    public IActionResult CreateNotificationPersonDetectedCamera(Guid homeId, Guid hardwareId)
    {
        var sensorRequest = new SensorRequest();
        var sensorEvent = "person-detected";
        var notifications = _homeLogic.CreateNotificationCamera(homeId, hardwareId, sensorEvent);
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
        var notifications = _homeLogic.CreateNotificationCamera(homeId, hardwareId, sensorEvent);
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

    [HttpPost]
    [Route("{homeId}/rooms")]
    [AuthorizationFilter("CanCreateRoom")]
    public IActionResult AddRoomToHome([FromRoute]Guid homeId, [FromBody]AddRoomRequest roomReq)
    {
        var roomName = roomReq.RoomName;
        var createdRoom = _homeLogic.AddRoom(homeId, roomName);
        
        var response = new NewRoomResponse()
        {
            Id = createdRoom.Id,
            Name = createdRoom.Name
        };
        return CreatedAtAction(nameof(AddRoomToHome), response);
    }

    [HttpGet]
    [Route("{homeId}/rooms")]
    [AuthorizationFilter("CanListDevices")]
    public IActionResult GetRooms(Guid homeId)
    {
        var rooms = _homeLogic.GetRooms(homeId);

        var getRoomsResponse = new GetRoomsResponse(rooms);

        return Ok(getRoomsResponse.ToArgs());
    }

    [HttpPut]
    [Route("{homeId}/rooms/{roomId}")]
    [AuthorizationFilter("CanAsociateDevices")]
    public IActionResult AddDeviceToRoom(Guid homeId, Guid roomId, [FromBody] AddDeviceToRoomRequest addDeviceToRoomRequest)
    {
        var hardwareId = addDeviceToRoomRequest.HardwareId;
        var room = _homeLogic.AddDeviceToRoom(homeId, hardwareId, roomId);

        var homeDevice = room.Devices.FirstOrDefault(x => x.HardwareId == hardwareId);

        var response = new DeviceRoomResponse
        {
            HardwareId = homeDevice.HardwareId,
            DeviceName = homeDevice.Name,
            RoomName = room.Name
        };

        return Ok(response);
    }

    [HttpPut]
    [Route("{homeId}")]
    [AuthorizationFilter("CanChangeHomeName")]
    public IActionResult ChangeHomeName(Guid homeId, [FromBody] string changeHomeNameRequest)
    {
        var name = changeHomeNameRequest;
        var home = _homeLogic.ChangeHomeName(homeId, name);

        var homeResponse = new HomeResponse
        {
            Name = home.Name,
            Location = home.Location,
            MemberCount = home.MemberCount,
            HomeOwner = home.HomeOwner,
            Latitude = home.Latitude,
            Longitude = home.Longitude
        };

        return Ok(homeResponse);
    }
}
