namespace Domain;

public class Company
{
    public string Name { get; set; }
    public string RUT { get; set; }
    public string Logo { get; set; }
    public Company(string name, string rut, string logo)
    {
        Name = name;
        RUT = rut;
        Logo = logo;
    }
}
