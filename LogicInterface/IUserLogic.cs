using Domain;

namespace LogicInterface;

public interface IUserLogic
{
    public User CreateAdmin(User user);
    
    public User CreateCompanyOwner(User user);
    
    public User AddCompanyToCompanyOwner(User companyOwner, Company company);
    
    public List<User> GetUsers();
}
