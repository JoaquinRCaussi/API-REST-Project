using Domain;

namespace Models;

public class HomeDeviceResponse
{
    public Guid HardwareId { get; set; }
    public Device? Device { get; set; }
}
