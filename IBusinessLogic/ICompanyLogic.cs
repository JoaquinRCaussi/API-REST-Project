using Domain;

namespace LogicInterface;

public interface ICompanyLogic
{
    public Company CreateCompany(Company companyToCreate);
     public List<Company> GetCompanies();
}
