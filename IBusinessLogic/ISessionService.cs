using Domain;

namespace LogicInterface;

public interface ISessionService
{
    User GetUserByToken(string token);
    void AddSession(Session session);
    Session Authenticate(string email, string password);
}
