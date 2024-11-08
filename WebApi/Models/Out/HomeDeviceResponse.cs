using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class HomeDeviceResponse
{
    public Guid HardwareId { get; set; }
    public Device? Device { get; set; }
    public bool? IsWindowSensorOpen { get; set; }
    public bool? IsLampOn { get; set; }
}
