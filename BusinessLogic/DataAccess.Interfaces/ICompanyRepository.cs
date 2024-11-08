using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface ICompanyRepository
{
    public Company CreateCompany(Company company);
    public List<Company> GetCompanies(string name, string ownerName);

    public bool ExistsCompany(Guid companyId);
}
