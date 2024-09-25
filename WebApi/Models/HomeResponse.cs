namespace WebApi.Models;

public class HomeResponse
{
    public string Location { get; set; }
    public int MemberCount { get; set; }
    public string Devices { get; set; }
    public string HomeOwner { get; set; }
    
    public HomeResponse(Domain.Home home)
    {
        Location = home.Location;
        MemberCount = home.MemberCount;
        Devices = home.Devices;
        HomeOwner = home.HomeOwner;
    }
}
