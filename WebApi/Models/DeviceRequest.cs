using Domain;

namespace WebApi.Models;

public class DeviceRequest
{
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }

    public DeviceRequest(Device device)
    {
        Name = device.Name;
        Model = device.Model;
        Description = device.Description;
        Photo = device.Photo;
    }

    public Device ToArgs()
    {
        return new Device(Name, Model, Description, Photo);
    }
}
