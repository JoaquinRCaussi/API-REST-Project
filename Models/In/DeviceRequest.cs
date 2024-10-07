using Domain;

namespace Models;

public class DeviceRequest
{
    public string? Name { get; set; }
    public string? Model { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }

    public Device ToArgs(Company company)
    {
        return new Device
        {
            Company = company,
            CompanyId = company.Id,
            Name = Name,
            Model = Model,
            DeviceType = DeviceType.Sensor,
            Description = Description,
            Photo = Photo
        };
    }
}
