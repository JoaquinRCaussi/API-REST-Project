using Domain;

namespace Models;

public class HomeRequest
{
    public required string Location { get; set; }
    public required int MemberCount { get; set; }
    public string? Devices { get; set; }
    public Guid HomeOwner { get; set; }

    public Home ToArgs()
    {
        return new Home
        {
            Location = Location,
            MemberCount = MemberCount,
            Devices = Devices,
            HomeOwner = HomeOwner
        };
    }
}
