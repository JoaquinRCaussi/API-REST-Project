using Domain;

namespace WebApi.Models;

public class AddCompanyToOwnerRequest
{
    public Company Company { get; set; }
    public User CompanyOwner { get; set; }

    public AddCompanyToOwnerRequest(User companyOwner, Company company)
    {
        Company = company;
        CompanyOwner = companyOwner;
    }
}
