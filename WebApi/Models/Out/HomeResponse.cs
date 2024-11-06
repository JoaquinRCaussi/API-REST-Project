using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class HomeResponse
{
    public required string Location { get; set; }
    public required string Latitude { get; set; }
    public required string Longitude { get; set; }
    public required int MemberCount { get; set; }
    public List<HomeDevice>? Devices { get; set; }
    public Guid HomeOwner { get; set; }

}
