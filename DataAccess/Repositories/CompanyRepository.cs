using DataAccess.Data;
using Domain;
using IDataAccess;

namespace DataAccess.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly HMDbContext _dbContext;
    public CompanyRepository(HMDbContext context)
    {
        _dbContext = context;
    }
    
    public Company CreateCompany(Company company)
    {
        throw new NotImplementedException();
    }

    public List<Company> GetCompanies(string name, string ownerName)
    {
        throw new NotImplementedException();
    }
}
