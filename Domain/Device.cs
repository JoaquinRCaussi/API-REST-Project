namespace Domain;

public class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Model { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }
}
