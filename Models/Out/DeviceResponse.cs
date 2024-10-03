using Domain;

namespace Models;

public class DeviceResponse
{
    public string? Name { get; set; }
    public string? Model { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }

    public DeviceResponse(Device device)
    {
        Name = device.Name;
        Model = device.Model;
        DeviceType = device.DeviceType;
        Description = device.Description;
        Photo = device.Photo;
    }
}
