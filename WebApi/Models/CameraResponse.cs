using Domain;

namespace WebApi.Models;

public class CameraResponse : DeviceResponse
{
    public bool Outdoors { get; set; }
    public bool Indoors { get; set; }
    public bool SupportMovementDetection { get; set; }
    public bool SupportPersonDetection { get; set; }

    public CameraResponse(Camera camera) : base(camera) 
    {
        Outdoors = camera.Outdoors;
        Indoors = camera.Indoors;
        SupportMovementDetection = camera.SupportMovementDetection;
        SupportPersonDetection = camera.SupportPersonDetection;
    }
}
