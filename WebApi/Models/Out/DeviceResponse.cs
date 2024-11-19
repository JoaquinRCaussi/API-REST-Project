
using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class DeviceResponse
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Model { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }

    public string CompanyName { get; set; }

    public DeviceResponse(Device device)
    {
        Id = device.Id;
        Name = device.Name;
        Model = device.Model;
        DeviceType = device.DeviceType;
        Description = device.Description;
        Photo = device.Photo;
        CompanyName = device.Company.Name;
    }
}
