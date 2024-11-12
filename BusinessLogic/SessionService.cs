using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;


namespace BusinessLogic;

public class SessionService : ISessionService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;

    public SessionService(IUserRepository userRepository, ISessionRepository sessionRepository)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
    }

    public User GetUserByToken(Guid token)
    {
        var session = _sessionRepository.FindByToken(token);

        if (session == null)
        {
            throw new UnauthorizedAccessException("Invalid token or token not found");
        }

        return session.User;
    }

    public void AddSession(Session session)
    {
        if (session == null)
        {
            throw new ArgumentNullException(nameof(session), "Session cannot be null");
        }

        if (_sessionRepository.FindByToken(session.Token) != null)
        {
            throw new UnauthorizedAccessException("A session with the same token already exists");
        }

        session.CreatedAt = DateTime.Now;
        _sessionRepository.AddSession(session);
    }

    public Session Authenticate(string email, string password)
    {

        var user = _userRepository.FindByMail(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = Guid.NewGuid();

        var session = new Session
        {
            User = user,
            Token = token,
            RoleID = user.RoleID
        };

        AddSession(session);

        return session;
    }

    public void Logout(Guid token)
    {
        var session = _sessionRepository.FindByToken(token);

        if (session == null)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }
        _sessionRepository.RemoveSession(session);
    }
}
