namespace Domain;

public class Notification
{
    public Guid Id { get; set; }
    public string? Event { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public Guid HardwareId { get; set; }
    public HomeDevice? HomeDevice { get; set; }
}
