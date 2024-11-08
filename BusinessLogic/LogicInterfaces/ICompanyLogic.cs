using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface ICompanyLogic
{
    public Company CreateCompany(Company companyToCreate);
    public List<Company> GetCompanies(string? name, string? ownerName);
}
