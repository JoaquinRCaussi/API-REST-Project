namespace BusinessLogic.Entities;

public class Room
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public List<HomeDevice> Devices { get; set; } = [];
}
