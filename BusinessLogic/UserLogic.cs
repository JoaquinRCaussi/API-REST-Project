using System.Text.RegularExpressions;
using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;


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
        var users = _userRepository.GetUsers();
        if (users.Count == 0)
        {
            throw new EmptyException("No users found.");
        }
        return users;
    }

    public User CreateAdmin(User user)
    {
        user.CreatedAt = DateTime.Now;
        if (!IsCorrectUserFormat(user))
        {
            throw new NotValidDataException("User data is not valid");
        }
        if (_userRepository.FindByMail(user.Email) != null)
        {
            throw new ConflictException("User with this email already exists");
        }
        return _userRepository.CreateAdmin(user);
    }

    public User CreateCompanyOwner(User user)
    {
        user.CreatedAt = DateTime.Now;
        if (!IsCorrectUserFormat(user))
        {
            throw new NotValidDataException("User data is not valid");
        }
        if (_userRepository.FindByMail(user.Email) != null)
        {
            throw new ConflictException("User with this email already exists");
        }
        return _userRepository.CreateCompanyOwner(user);
    }

    public User CreateHomeOwner(User user)
    {
        user.CreatedAt = DateTime.Now;
        if (!IsCorrectUserFormat(user))
        {
            throw new NotValidDataException("User data is not valid");
        }
        if (!IsCorrectImagePath(user.ImagePath))
        {
            throw new NotValidDataException("Image path must be one of these (.jpg, .jpeg, .png, .gif).");
        }
        if (_userRepository.FindByMail(user.Email) != null)
        {
            throw new ConflictException("User with this email already exists");
        }
        return _userRepository.CreateHomeOwner(user);
    }

    public User GetUser(Guid userId)
    {
        if (!_userRepository.ExistUser(userId))
        {
            throw new NotValidDataException("User does not exist");
        }
        return _userRepository.GetUser(userId);
    }

    public User FindByMail(string mail)
    {
        var user = _userRepository.FindByMail(mail);
        if (user == null)
        {
            throw new NotValidDataException("User does not exist");
        }
        return user;
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

    public List<Notification> GetNotifications(Guid userId)
    {
        var notifications = _userRepository.GetNotifications(userId);

        if (notifications.Count == 0)
        {
            throw new EmptyException("No notifications found.");
        }

        return notifications;
    }

    public (List<User> Users, int TotalResults) GetUsersFiltered(string? role, string? fullName, int pageNumber, int pageSize)
    {
        // Llamar al UserRepository para obtener los usuarios filtrados
        var users = _userRepository.GetUsersFiltered(role, fullName);

        // Total de resultados antes de paginar
        var totalResults = users.Count;

        // Lógica de paginación
        var paginatedUsers = users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (paginatedUsers, totalResults);
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
    public bool IsCorrectImagePath(string imagePath)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        var fileExtension = Path.GetExtension(imagePath).ToLower();

        return validExtensions.Contains(fileExtension);
    }
}
