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

        if (!IsCorrectImagePath(companyToCreate.Logo))
        {
            throw new NotValidDataException("Image path must be one of these (.jpg, .jpeg, .png, .gif).");
        }

        var companies = _companyRepository.GetCompanies(companyToCreate.Name, companyToCreate.Owner.Name);
        if (companies.Count > 0)
        {
            throw new ConflictException("The company already exists");
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

        List<Company> companies = _companyRepository.GetCompanies(name, ownerName);
        if (companies.Count == 0)
        {
            throw new EmptyException("No companies found.");
        }

        return companies;
    }

    private bool IsFormatNotCorrect(Company company)
    {
        return company.Name == "" || company.RUT == "" || company.Logo == "";
    }
    public bool IsCorrectImagePath(string imagePath)
    {
        string[] validExtensions = [".jpg", ".jpeg", ".png", ".gif"];

        var fileExtension = Path.GetExtension(imagePath).ToLower();

        return validExtensions.Contains(fileExtension);
    }
}
