using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;

    public DeviceController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic;
    }

    // Ruta para dispositivos generales
    [HttpPost]
    [Route("devices")]
    public IActionResult CreateDevice([FromBody] DeviceRequest device)
    {
        Device deviceToCreate = device.ToArgs();
        Device createdDevice = _deviceLogic.CreateDevice(deviceToCreate);
        var response = new DeviceResponse(createdDevice);
        return Ok(response);
    }

    // Ruta específica para cámaras (api/cameras)
    [HttpPost]
    [Route("cameras")]
    public IActionResult CreateCamera([FromBody] CameraRequest camera)
    {
        var cameraToCreate = (Camera)camera.ToArgs();
        Camera createdCamera = _deviceLogic.CreateCamera(cameraToCreate);
        var response = new CameraResponse(createdCamera);
        return Ok(response);
    }
}
