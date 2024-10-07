using Domain;

namespace Models;

public class CameraRequest : DeviceRequest
{
    public bool? Outdoors { get; set; }
    public bool? Indoors { get; set; }
    public bool? SupportMovementDetection { get; set; }
    public bool? SupportPersonDetection { get; set; }

    public Camera ToArgs(Company company)
    {
        return new Camera
        {
            Company = company,
            CompanyId = company.Id,
            Name = Name,
            Model = Model,
            DeviceType = DeviceType.Camera,
            Description = Description,
            Photo = Photo,
            Outdoors = Outdoors ?? false,
            Indoors = Indoors ?? false,
            SupportMovementDetection = SupportMovementDetection ?? false,
            SupportPersonDetection = SupportPersonDetection ?? false
        };
    }
}
