namespace Domain;

public class HomeDevice
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid HardwareId { get; init; } = Guid.NewGuid();
    public Guid DeviceId { get; set; }
    public string? Name { get; set; }
    public bool State { get; set; } = false;
    public Device? Device { get; set; }
}
