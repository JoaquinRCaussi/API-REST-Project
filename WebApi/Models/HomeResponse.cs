using Domain;

namespace WebApi.Models;

public class HomeResponse
{
    public required string Location { get; set; }
    public required int MemberCount { get; set; }
    public string? Devices { get; set; }
    public Guid HomeOwner { get; set; }
}
