using DataAccess.Data;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

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
        owner.CompanyID = company.Id;
        owner.Company = company;
        _dbContext.Users?.Update(owner);
        _dbContext.Companies?.Add(company);
        _dbContext.SaveChanges();
        return company;
    }

    public List<Company> GetCompanies(string name, string ownerName)
    {

        var companies = _dbContext.Companies?
            .Include(x => x.Owner)
            .ToList();

        var filteredCompanies = companies?.Where(c => c.Name.Contains(name) && c.Owner.Name.Contains(ownerName)).ToList();

        return filteredCompanies;
    }
    
    public bool ExistsCompany(Guid companyId)
    {
        return _dbContext.Companies?.Any(x => x.Id == companyId) ?? false;
    }
}
