using Domain;

namespace WebApi.Models;

public class AddCompanyToOwnerResponse
{
    public User CompanyOwner { get; set; }
    public Company Company { get; set; }
    
    public AddCompanyToOwnerResponse(User companyOwner, Company company)
    {
        Company = company;
        CompanyOwner = companyOwner;
    }
}
