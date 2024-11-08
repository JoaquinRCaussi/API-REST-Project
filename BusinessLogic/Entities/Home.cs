
namespace BusinessLogic.Entities;

public class Home
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Location { get; set; }
    public required string Latitude { get; set; }
    public required string Longitude { get; set; }
    public required int MemberCount { get; set; }
    public List<HomeDevice>? Devices { get; set; }
    public Guid HomeOwner { get; set; }
    public User? Owner { get; set; }
    public List<User>? Members { get; set; } = [];
    public List<Room>? Rooms { get; set; } = [];

    public virtual List<MemberSetting> MemberSettings { get; set; } = [];
}
