using Domain;
using IDataAccess;
using LogicInterface;

namespace BusinessLogic;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;
    
    public UserLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public List<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }
    
    public User CreateAdmin(User user)
    {
        return _userRepository.CreateAdmin(user);
    }
    
    public User CreateCompanyOwner(User user)
    {
        return _userRepository.CreateCompanyOwner(user);
    }
    
    public User AddCompanyToCompanyOwner(User user, Company company)
    {
        return _userRepository.AddCompanyToCompanyOwner(user, company);
    }
}
