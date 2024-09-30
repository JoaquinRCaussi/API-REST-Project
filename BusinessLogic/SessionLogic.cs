using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class    SessionLogic : ISessionLogic
{
    private readonly IUserRepository _repository;
    //Aca tambien servicio de sesion. Uso los dos. El de usuario para encontarar usarios
    //y el de sesion para agregar la sesion
    private User? _currentUser;

    public SessionLogic(IUserRepository repository)
    {
        _repository = repository;
    }

    public User Authenticate(string mail, string password)
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

        return session.User;
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
