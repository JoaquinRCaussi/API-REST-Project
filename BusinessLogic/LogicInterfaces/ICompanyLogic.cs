using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface ICompanyLogic
{
    public Company CreateCompany(Company companyToCreate);
    (List<Company> Companies, int TotalResults) GetCompanies(string? name, string? ownerName, int pageNumber, int pageSize);
}
