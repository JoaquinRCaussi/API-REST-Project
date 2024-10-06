using Domain;

namespace IBusinessLogic;

public interface IUserLogic
{
    public User CreateAdmin(User user);

    public User CreateCompanyOwner(User user);
    public User CreateHomeOwner(User user);

    public User AddCompanyToCompanyOwner(User companyOwner, Company company);

    public List<User> GetUsers();

    public User GetUser(Guid userId);

    public User FindByMail(string mail);

    public bool ExistUser(Guid userId);

    public User DeleteUser(Guid userId);

    public User AuthenticateUser(string mail, string password);
    
    public List<Notification> GetNotifications(Guid userId);
}
