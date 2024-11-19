using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface ICompanyRepository
{
    public Company CreateCompany(Company company);
    (List<Company> Companies, int TotalResults) GetCompanies(string? name, string? ownerName, int pageNumber, int pageSize);

    public bool ExistsCompany(Guid companyId);
}
