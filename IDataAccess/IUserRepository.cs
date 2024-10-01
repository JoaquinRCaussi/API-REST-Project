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

    public User FindByMail(string mail);

    public bool ExistUserByToken(Guid userToken);

    public bool ExistUser(Guid userId);

    public User DeleteUser(Guid userId);
}
