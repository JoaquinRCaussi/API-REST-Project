using System.Text.RegularExpressions;
using Domain;
using IBusinessLogic;
using IDataAccess;

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
        if (!IsCorrectUserFormat(user))
        {
            throw new NotValidDataException("User data is not valid");
        }
        return _userRepository.CreateAdmin(user);
    }

    public User CreateCompanyOwner(User user)
    {
        if (!IsCorrectUserFormat(user))
        {
            throw new NotValidDataException("User data is not valid");
        }
        return _userRepository.CreateCompanyOwner(user);
    }

    public User CreateHomeOwner(User user)
    {
        if (!IsCorrectUserFormat(user))
        {
            throw new NotValidDataException("User data is not valid");
        }
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

    public User AuthenticateUser(string mail, string password)
    {
        return _userRepository.AuthenticateUser(mail, password);
    }

    private bool IsCorrectUserFormat(User user)
    {
        if (!IsCorrectEmail(user.Email))
        {
            throw new NotValidDataException("Email is not valid");
        }
        return user.Name.Length > 0 && user.LastName.Length > 0 && user.Email.Length > 0 && user.Password.Length > 0;
    }

    private bool IsCorrectEmail(string email)
    {
        var correctPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, correctPattern);
    }
}
