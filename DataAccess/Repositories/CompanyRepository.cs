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
        var owner = _dbContext.Users?.FirstOrDefault(u => u.Id == company.Owner.Id);

        if (owner == null)
        {
            throw new Exception("Owner not found");
        }

        company.Owner = owner;
        _dbContext.Companies?.Add(company);
        _dbContext.SaveChanges();
        return company;
    }

    public List<Company> GetCompanies(string name, string ownerName)
    {
        return _dbContext.Companies?.Where(x => x.Name.Contains(name) && x.Owner.Name.Contains(ownerName)).ToList()!;
    }
}
