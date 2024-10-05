using Domain;

namespace Models;

public class CameraRequest : DeviceRequest
{
    public bool? Outdoors { get; set; }
    public bool? Indoors { get; set; }
    public bool? SupportMovementDetection { get; set; }
    public bool? SupportPersonDetection { get; set; }

    public CameraRequest(Camera camera) : base(camera)  // Llama al constructor base de DeviceRequest
    {
        Outdoors = camera.Outdoors;
        Indoors = camera.Indoors;
        SupportMovementDetection = camera.SupportMovementDetection;
        SupportPersonDetection = camera.SupportPersonDetection;
    }

    public override Device ToArgs(Company company)
    {
        return new Camera
        {
            Company = company,
            Name = Name,
            Model = Model,
            DeviceType = DeviceType,
            Description = Description,
            Photo = Photo,
            Outdoors = Outdoors ?? false,
            Indoors = Indoors ?? false,
            SupportMovementDetection = SupportMovementDetection ?? false,
            SupportPersonDetection = SupportPersonDetection ?? false
        };
    }
}
