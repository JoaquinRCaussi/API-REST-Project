using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

[ApiController]
[Route("api/cameras")]
public class CameraController : ControllerBase
{
    private readonly IDeviceLogic _cameraLogic;

    public CameraController(IDeviceLogic cameraLogic)
    {
        _cameraLogic = cameraLogic;
    }

    [HttpPost]
    public IActionResult CreateCamera([FromBody] CameraRequest camera)
    {
        var cameraToCreate = (Camera)camera.ToArgs();
        Camera createdCamera = _cameraLogic.CreateCamera(cameraToCreate);
        var response = new CameraResponse(createdCamera);
        return Ok(response);
    }

}
