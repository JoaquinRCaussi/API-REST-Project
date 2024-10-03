using Domain;

namespace IBusinessLogic;

public interface ICompanyLogic
{
    public Company CreateCompany(Company companyToCreate);
    public List<Company> GetCompanies(string? name, string? ownerName);
}
