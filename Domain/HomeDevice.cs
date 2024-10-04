namespace Domain;

public class HomeDevice
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid DeviceId { get; set; }
    public bool state { get; set; } = false;
    public Device? Device { get; set; }
}
