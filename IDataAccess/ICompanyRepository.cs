using Domain;

namespace IDataAccess;

public interface ICompanyRepository
{
    public Company CreateCompany(Company company);
}
