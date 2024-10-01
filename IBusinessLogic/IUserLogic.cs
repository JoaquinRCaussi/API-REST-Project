using Domain;

namespace LogicInterface;

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
    public bool IsTheCorrectUser(Guid userToken);
}
