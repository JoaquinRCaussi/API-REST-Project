using Domain;

namespace Models;

public class DeviceRequest
{
    public string? Name { get; set; }
    public string? Model { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }

    public DeviceRequest(Device device)
    {
        Name = device.Name;
        Model = device.Model;
        DeviceType = device.DeviceType;
        Description = device.Description;
        Photo = device.Photo;
    }

    public virtual Device ToArgs()
    {
        return new Device
        {
            Name = Name,
            Model = Model,
            DeviceType = DeviceType,
            Description = Description,
            Photo = Photo
        };
    }
}
