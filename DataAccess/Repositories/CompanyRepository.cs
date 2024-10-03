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
        _dbContext.Companies?.Add(company);
        _dbContext.SaveChanges();
        return company;
    }

    public List<Company> GetCompanies(string name, string ownerName)
    {
        return _dbContext.Companies?.ToList()!;
    }
}
