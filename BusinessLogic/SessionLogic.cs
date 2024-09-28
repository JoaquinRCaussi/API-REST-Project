using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class SessionLogic : ISessionLogic
{
    private readonly ISessionRepository _repository;
    private User? _currentUser;

    public SessionLogic(ISessionRepository repository)
    {
        _repository = repository;
    }

    public Guid Authenticate(string mail, string password)
    {
        var user = _repository.FindByMail(mail);

        if (user == null || user.Password != password)
        {
            throw new Exception("Invalid email or password.");
        }

        var session = new Session
        {
            User = user,
            UserID = user.Id,
            RoleID = user.Role,
        };

        _repository.AddSession(session);

        return session.Token;
    }

    public User GetCurrentUser(Guid? token = null)
    {
        if (token == null)
        {
            return _currentUser;
        }
        
        _currentUser = _repository.FindByToken(token.Value);
        return _currentUser;
    }
}
