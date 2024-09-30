//using WebApi.Models;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;

[ApiController]
[Route("api/device")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;

    public DeviceController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic;
    }

    // Endpoint para crear un dispositivo
    [HttpPost]
    public IActionResult CreateDevice(Device device)
    {
        if (string.IsNullOrEmpty(device.Name) || string.IsNullOrEmpty(device.Model))
        {
            return BadRequest("Faltan propiedades.");
        }

        var newDevice = _deviceLogic.CreateDevice(device);
        return Ok(newDevice);
    }
}
