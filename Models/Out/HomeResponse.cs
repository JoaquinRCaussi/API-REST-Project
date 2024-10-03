using Domain;

namespace Models;

public class HomeResponse
{
    public required string Location { get; set; }
    public required int MemberCount { get; set; }
    public List<Device>? Devices { get; set; }
    public Guid HomeOwner { get; set; }

}
