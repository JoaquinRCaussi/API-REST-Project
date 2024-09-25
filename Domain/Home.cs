namespace Domain;

public class Home
{
    public string Location { get; set; }
    public int MemberCount { get; set; }
    public string Devices { get; set; }
    public User HomeOwner { get; set; }
    
    public Home(string location, int memberCount, string devices, User homeOwner)
    {
        Location = location;
        MemberCount = memberCount;
        Devices = devices;
        HomeOwner = homeOwner;
    }
}
