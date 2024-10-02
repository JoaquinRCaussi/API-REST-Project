using Domain;
using IDataAccess;
using IBusinessLogic;

namespace BusinessLogic;

public class SessionService : ISessionService
{
    private readonly IUserRepository _userRepository;
    private static readonly List<Session> _sessions = [];

    public SessionService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User GetUserByToken(string token)
    {
        var session = _sessions.FirstOrDefault(s => s.Token == token);

        if (session == null)
        {
            throw new Exception("Session not found");
        }

        return session.User;
    }

    public void AddSession(Session session)
    {
        if (session == null)
        {
            throw new ArgumentNullException(nameof(session), "Session cannot be null");
        }

        if (_sessions.Any(s => s.Token == session.Token))
        {
            throw new InvalidOperationException("A session with the same token already exists");
        }

        _sessions.Add(session);
    }

    public Session Authenticate(string email, string password)
    {

        var user = _userRepository.FindByMail(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = Guid.NewGuid().ToString(); // Genera un token único
        var session = new Session
        {
            User = user,
            Token = token,
            RoleID = user.RoleID
        };

        AddSession(session);

        return session; // Devuelve la sesión creada
    }
}
