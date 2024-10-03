using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyLogic : ICompanyLogic
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;

    public CompanyLogic(ICompanyRepository companyRepository, IUserRepository userRepository)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    public Company CreateCompany(Company companyToCreate)
    {
        var owner = _userRepository.GetUser(companyToCreate.Owner.Id);

        if (owner.CompanyID != null)
        {
            throw new ConflictException("The owner already has a company");
        }

        if (IsFormatNotCorrect(companyToCreate))
        {
            throw new NotValidDataException("The name and RUT are required");
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

    private bool IsFormatNotCorrect(Company company)
    {
        return company.Name == "" || company.RUT == "" || company.Logo == "";
    }
}
