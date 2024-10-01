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

    public User CreateHomeOwner(User user)
    {
        return _userRepository.CreateHomeOwner(user);
    }

    public User AddCompanyToCompanyOwner(User user, Company company)
    {
        return _userRepository.AddCompanyToCompanyOwner(user, company);
    }

    public User GetUser(Guid userId)
    {
        return _userRepository.GetUser(userId);
    }

    public User FindByMail(string mail)
    {
        return _userRepository.FindByMail(mail);
    }

    public bool ExistUser(Guid userId)
    {
        return _userRepository.ExistUser(userId);
    }

    public User DeleteUser(Guid userId)
    {
        if (!_userRepository.ExistUser(userId))
        {
            throw new NotValidDataException("User does not exist");
        }
        return _userRepository.DeleteUser(userId);
    }

    public bool IsTheCorrectUser(Guid userToken)
    {
        return _userRepository.ExistUserByToken(userToken);
    }
}
