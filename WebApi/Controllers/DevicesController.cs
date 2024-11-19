using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[ApiController]
[Route("api/devices")]
[AuthenticationFilter]
public class DevicesController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;

    public DevicesController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic;

    }

    [HttpPost]
    [Route("smartLamp")]
    [AuthorizationFilter("CanCreateADevice")]
    public IActionResult CreateSmartLamp([FromBody] SmartLampRequest device)
    {
        var user = HttpContext.Items[0] as User;
        var deviceToCreate = device.ToArgs(user.Company);
        var createdDevice = _deviceLogic.CreateDevice(deviceToCreate);
        var response = new DeviceResponse(createdDevice);
        return CreatedAtAction(nameof(CreateSmartLamp), new { id = createdDevice.Id }, response);
    }

    [HttpPost]
    [Route("movementSensor")]
    [AuthorizationFilter("CanCreateADevice")]
    public IActionResult CreateMovementSensor([FromBody] MovementSensorRequest device)
    {
        var user = HttpContext.Items[0] as User;
        Device deviceToCreate = device.ToArgs(user.Company);
        Device createdDevice = _deviceLogic.CreateDevice(deviceToCreate);
        var response = new DeviceResponse(createdDevice);
        return CreatedAtAction(nameof(CreateMovementSensor), new { id = createdDevice.Id }, response);
    }

    [HttpPost]
    [Route("windowSensor")]
    [AuthorizationFilter("CanCreateADevice")]
    public IActionResult CreateDevice([FromBody] DeviceRequest device)
    {
        var user = HttpContext.Items[0] as User;
        Device deviceToCreate = device.ToArgs(user.Company);
        Device createdDevice = _deviceLogic.CreateDevice(deviceToCreate);
        var response = new DeviceResponse(createdDevice);
        return CreatedAtAction(nameof(CreateDevice), new { id = createdDevice.Id }, response);
    }

    [HttpPost]
    [Route("camera")]
    [AuthorizationFilter("CanCreateADevice")]
    public IActionResult CreateCamera([FromBody] CameraRequest camera)
    {
        var user = HttpContext.Items[0] as User;
        var cameraToCreate = (Camera)camera.ToArgs(user.Company);
        Camera createdCamera = _deviceLogic.CreateCamera(cameraToCreate);
        var response = new CameraResponse(createdCamera);
        return CreatedAtAction(nameof(CreateCamera), new { id = createdCamera.Id }, response);
    }

    [HttpGet]
    public IActionResult GetDevices(
    [FromQuery] string? name,
    [FromQuery] string? model,
    [FromQuery] string? companyName,
    [FromQuery] DeviceType? deviceType,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var (devices, totalResults) = _deviceLogic.GetDevices(name, model, companyName, deviceType, pageNumber, pageSize);

        var response = devices.Select(d => new DeviceResponse(d)).ToList();

        return Ok(new
        {
            TotalResults = totalResults,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Devices = response
        });
    }

    [HttpGet]
    [Route("types")]
    public IActionResult GetDevicesTypes()
    {
        var deviceTypes = _deviceLogic.GetDevicesTypes();
        return Ok(deviceTypes);
    }
}
