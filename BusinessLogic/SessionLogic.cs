using Domain;
using IBusinessLogic;
using IDataAccess;
using Models;

namespace BusinessLogic;

public class    SessionLogic : ISessionLogic
{
    private readonly IUserRepository _repository;
    private readonly ISessionRepository _sessionRepository;
    private User? _currentUser;

    public SessionLogic(IUserRepository repository, ISessionRepository sessionRepository)
    {
        _repository = repository;
        _sessionRepository = sessionRepository;
    }

    public AuthenticationResult Authenticate(string mail, string password)
    {
        var user = _repository.FindByMail(mail);

        if (user == null || user.Password != password)
        {
            throw new Exception("Invalid email or password.");
        }

        var session = new Session
        {
            UserID = user.Id,
            RoleID = user.Role,
        };

        _sessionRepository.AddSession(session);

        var result = new AuthenticationResult
        {
            UserId = user.Id,
            RoleId = user.Role,
        };
        
        return result;
    }

    public User GetCurrentUser(Guid? token = null)
    {
        if (token == null)
        {
            return _currentUser;
        }

        var session = _sessionRepository.FindByToken(token.Value);

        if (session == null)
        {
            throw new Exception("Invalid token.");
        }
        
        _currentUser = _repository.GetUser(session.UserID);
        
        return _currentUser;
    }
}
