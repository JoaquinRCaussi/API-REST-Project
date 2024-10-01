using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;

    public DeviceController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic;
    }

    [HttpPost]
    public IActionResult CreateDevice([FromBody] DeviceRequest device)
    {
        Device deviceToCreate = device.ToArgs();
        Device createdDevice = _deviceLogic.CreateDevice(deviceToCreate);
        var response = new DeviceResponse(createdDevice);
        return Ok(response);
    }
}
