using Domain;

namespace WebApi.Models;

public class CameraRequest : DeviceRequest
{
    public bool Outdoors { get; set; }
    public bool Indoors { get; set; }
    public bool SupportMovementDetection { get; set; }
    public bool SupportPersonDetection { get; set; }

    public CameraRequest(Camera camera) : base(camera)  // Llama al constructor base de DeviceRequest
    {
        Outdoors = camera.Outdoors;
        Indoors = camera.Indoors;
        SupportMovementDetection = camera.SupportMovementDetection;
        SupportPersonDetection = camera.SupportPersonDetection;
    }

    public override Device ToArgs()
    {
        return new Camera(Name, Model, Description, Photo, DeviceType, Outdoors, Indoors, SupportMovementDetection, SupportPersonDetection);
    }
}
