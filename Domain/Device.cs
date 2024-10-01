namespace Domain;

public class Device
{
    public string Name { get; set; }
    public string Model { get; set; }
    public DeviceType DeviceType { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }

    public Device(string name, string model, DeviceType deviceType, string description, string photo)
    {
        Name = name;
        Model = model;
        DeviceType = deviceType;
        Description = description;
        Photo = photo;
    }
}
