
using BusinessLogic.Entities;

namespace WebApi.Models.In;

public class HomeRequest
{
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required string Latitude { get; set; }
    public required string Longitude { get; set; }
    public required int MemberCount { get; set; }
    public Guid HomeOwner { get; set; }

    public Home ToArgs()
    {
        return new Home
        {
            Name = Name,
            Location = Location,
            MemberCount = MemberCount,
            Latitude = Latitude,
            Longitude = Longitude,
        };
    }
}
