using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthenticationFilter]
public class DevicesController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;

    public DevicesController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic;
        
    }


    [HttpPost]
    [AuthorizationFilter("CanCreateADevice")]
    public IActionResult CreateDevice([FromBody] DeviceRequest device)
    {
        var user = HttpContext.Items[0] as User;
        Device deviceToCreate = device.ToArgs(user.Company);
        Device createdDevice = _deviceLogic.CreateDevice(deviceToCreate);
        var response = new DeviceResponse(createdDevice);
        return Ok(response);
    }

    [HttpPost]
    [Route("cameras")]
    [AuthorizationFilter("CanCreateADevice")]
    public IActionResult CreateCamera([FromBody] CameraRequest camera)
    {
        var user = HttpContext.Items[0] as User;
        var cameraToCreate = (Camera)camera.ToArgs(user.Company);
        Camera createdCamera = _deviceLogic.CreateCamera(cameraToCreate);
        var response = new CameraResponse(createdCamera);
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetDevices([FromQuery] string? name, [FromQuery] string? model, [FromQuery] string? CompanyName, [FromQuery] DeviceType? DeviceType)
    {
        var devices = _deviceLogic.GetDevices(name, model, CompanyName, DeviceType).Select(d => new DeviceResponse(d)).ToList();
        return Ok(devices);
    }

    [HttpGet]
    [Route("types")]
    public IActionResult GetDevicesTypes()
    {
        var deviceTypes = _deviceLogic.GetDevicesTypes();
        return Ok(deviceTypes);
    }
}
