namespace Domain;

public class Device
{
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }

    public Device(string name, string model, string description, string photo)
    {
        Name = name;
        Model = model;
        Description = description;
        Photo = photo;
    }
}
