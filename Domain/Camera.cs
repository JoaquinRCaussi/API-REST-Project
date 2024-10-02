namespace Domain;

public class Camera : Device
{
    public bool Outdoors { get; set; }
    public bool Indoors { get; set; }
    public bool SupportMovementDetection { get; set; }
    public bool SupportPersonDetection { get; set; }

    public Camera(string name, string model, string description, string photo, DeviceType deviceType,
                  bool outdoors, bool indoors,
                  bool supportMovementDetection, bool supportPersonDetection)
        : base(name, model, deviceType, description, photo)
    {
        Outdoors = outdoors;
        Indoors = indoors;
        SupportMovementDetection = supportMovementDetection;
        SupportPersonDetection = supportPersonDetection;
    }

}
