namespace Domain;

public class Home
{
    public Guid Id { get; set; }
    public required string Location { get; set; }
    public required int MemberCount { get; set; }
    public required string Devices { get; set; }
    public required Guid HomeOwner { get; set; }
    public User? Owner { get; set; }
    public List<User>? Members { get; set; }
}
