using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using DataAccess.Data;
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
            throw new EntityNotFoundException("Owner not found");
        }

        company.Owner = owner;
        owner.CompanyID = company.Id;
        owner.Company = company;
        _dbContext.Users?.Update(owner);
        _dbContext.Companies?.Add(company);
        _dbContext.SaveChanges();
        return company;
    }

    public (List<Company> Companies, int TotalResults) GetCompanies(string? name, string? ownerName, int pageNumber, int pageSize)
    {
        var companies = _dbContext.Companies?
            .Include(c => c.Owner)
            .Where(c => (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(ownerName) || c.Owner.Name.Contains(ownerName)));

        var totalResults = companies == null ? 0 : companies.Count();

        companies = companies?
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var companyList = companies == null ? [] : companies.ToList();
        return (companyList, totalResults);
    }

    public bool ExistsCompany(Guid companyId)
    {
        return _dbContext.Companies?.Any(x => x.Id == companyId) ?? false;
    }
}
