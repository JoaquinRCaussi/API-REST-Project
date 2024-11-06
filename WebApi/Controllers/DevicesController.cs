using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using WebApi.Models.In;
using WebApi.Models.Out;

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
        return CreatedAtAction(nameof(CreateDevice), new { id = createdDevice.Id }, response);
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
        var devices = _deviceLogic.GetDevices(name, model, companyName, deviceType);

        var totalResults = devices.Count;

        var paginatedDevices = devices
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = paginatedDevices.Select(d => new DeviceResponse(d)).ToList();

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
