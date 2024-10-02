using Domain;

namespace IDataAccess;

public interface ICompanyRepository
{
    public Company CreateCompany(Company company);
    public List<Company> GetCompanies(object o, object o1);
}
