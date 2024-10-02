using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyLogic : ICompanyLogic
{
    private readonly ICompanyRepository _companyRepository;
    
    public CompanyLogic(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    
    public Company CreateCompany(Company companyToCreate)
    {
        if (companyToCreate.Owner.CompanyID != Guid.Empty)
        {
            throw new ConflictException("The owner already has a company");
        }
        return _companyRepository.CreateCompany(companyToCreate);
    }

    public List<Company> GetCompanies(string? name, string? ownerName)
    {
        if (name == null)
        {
            name = "";
        }

        if (ownerName == null)
        {
            ownerName = "";
        }
        return _companyRepository.GetCompanies(name, ownerName);
    }
}
