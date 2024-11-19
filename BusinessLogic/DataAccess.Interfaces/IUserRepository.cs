using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface IUserRepository
{
    public List<User> GetUsers();

    public User GetUser(Guid userId);

    public User CreateAdmin(User user);

    public User CreateCompanyOwner(User user);

    public User CreateHomeOwner(User user);

    public User AddCompanyToCompanyOwner(User user, Company company);

    public User FindByMail(string mail);

    public User AuthenticateUser(string mail, string password);

    public bool ExistUser(Guid userId);

    public User DeleteUser(Guid userId);

    public List<Notification> GetNotifications(Guid userId);

    public (List<User> Users, int TotalResults) GetUsersFiltered(string? role, string? fullName, int pageNumber, int pageSize);
}
