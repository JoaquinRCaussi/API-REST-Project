using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using WebApi.Models.In;

namespace WebApi.Controllers;

[Route("api/import-devices")]
[ApiController]
[AuthenticationFilter]
public class ImportDevicesController : ControllerBase
{
    private readonly IDeviceImportLogic _deviceImportLogic;
    
    public ImportDevicesController(IDeviceImportLogic deviceImportLogic)
    {
        _deviceImportLogic = deviceImportLogic;
    }
    
    [HttpPost]
    public IActionResult ImportDevices([FromBody] ImportDevicesRequest importDevices)
    {
        if(importDevices.CompanyName == null || importDevices.AssemblyPath == null)
        {
            return BadRequest("Company Name and Assembly Path are required");
        }
        
        _deviceImportLogic.ImportDevices(importDevices.CompanyName, importDevices.AssemblyPath);
        return CreatedAtAction(nameof(ImportDevices), "Devices Imported");
    }
}
