namespace Domain;

public interface IUserLogic
{
    public User CreateAdmin(User user);
    
    public User CreateCompanyOwner(User user);
    
    public User AddCompanyToCompanyOwner(User companyOwner, Company company);
}
