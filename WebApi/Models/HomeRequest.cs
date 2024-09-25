using Domain;

namespace WebApi.Models;

public class HomeRequest
{
    public string Location { get; set; }
    public int MemberCount { get; set; }
    public string Devices { get; set; }
    public string HomeOwner { get; set; }
    
    public HomeRequest(Home home)
    {
        Location = home.Location;
        MemberCount = home.MemberCount;
        Devices = home.Devices;
        HomeOwner = home.HomeOwner;
    }
    
    public Home ToArgs()
    {
        return new Home(Location, MemberCount, Devices, HomeOwner);
    }
}
