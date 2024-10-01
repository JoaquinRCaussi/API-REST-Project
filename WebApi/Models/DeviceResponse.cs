using Domain;

namespace WebApi.Models;

public class DeviceResponse
{
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }

    public DeviceResponse(Device device)
    {
        Name = device.Name;
        Model = device.Model;
        Description = device.Description;
        Photo = device.Photo;
    }
}
