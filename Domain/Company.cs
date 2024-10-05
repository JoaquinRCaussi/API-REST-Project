namespace Domain;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RUT { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public  User? Owner { get; set; }
    public Guid OwnerId { get; set; }


}
