using Domain;

namespace WebApi.Models;

public class CompanyResponse
{
    public string Name { get; set; }
    public string? RUT { get; set; }
    public string Logo { get; set; }
    public CompanyResponse(Company company)
    {
        Name = company.Name;
        RUT = company.RUT;
        Logo = company.Logo;
    }
}
