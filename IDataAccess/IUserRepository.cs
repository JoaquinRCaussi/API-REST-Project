using Domain;

namespace IDataAccess;

public interface IUserRepository
{
    public List<User> GetUsers();

    public User GetUser(Guid userId);

    public User CreateAdmin(User user);

    public User CreateCompanyOwner(User user);

    public User CreateHomeOwner(User user);

    public User AddCompanyToCompanyOwner(User user, Company company);
    
}
