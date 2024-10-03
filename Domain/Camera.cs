namespace Domain;

public class Camera : Device
{
    public bool Outdoors { get; set; }
    public bool Indoors { get; set; }
    public bool SupportMovementDetection { get; set; }
    public bool SupportPersonDetection { get; set; }
}
