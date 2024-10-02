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
        return _companyRepository.CreateCompany(companyToCreate);
    }

    public List<Company> GetCompanies(string? name, string? ownerName)
    {
        throw new NotImplementedException();
    }
}
