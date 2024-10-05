using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthenticationFilter]
public class DeviceController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;

    public DeviceController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic;
    }

    
    [HttpPost]
    [Route("devices")]
    [AuthorizationFilter("CanCreateDevice")]
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
    [AuthorizationFilter("CanCreateDevice")]
    public IActionResult CreateCamera([FromBody] CameraRequest camera)
    {
        var user = HttpContext.Items[0] as User;
        var cameraToCreate = (Camera)camera.ToArgs(user.Company);
        Camera createdCamera = _deviceLogic.CreateCamera(cameraToCreate);
        var response = new CameraResponse(createdCamera);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("devices")]
    public IActionResult GetDevices()
    {
        throw new Exception();
    }
}
