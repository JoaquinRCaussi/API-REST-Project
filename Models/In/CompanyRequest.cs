using Domain;

namespace Models;

public class CompanyRequest
{
    public CompanyRequest(string name, string RUT, string logo)
    {
        Name = name;
        this.RUT = RUT;
        this.logo = logo;
    }

    public string Name { get; set; }
    public string RUT { get; set; }
    public string logo { get; set; }

    public Company ToArgs(User? user)
    {
        return new Company
        {
            Name = Name, RUT = RUT, Logo = logo, Owner = user
        };
    }
}
