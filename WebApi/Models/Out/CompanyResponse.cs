

using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class CompanyResponse
{
    public string Name { get; set; }
    public string? RUT { get; set; }
    public string Logo { get; set; }

    public string OwnerName { get; set; }
    public string OwnerEmail { get; set; }

    public CompanyResponse(Company company)
    {
        Name = company.Name;
        RUT = company.RUT;
        Logo = company.Logo;
        OwnerName = company.Owner.Name;
        OwnerEmail = company.Owner.Email;
    }
}
