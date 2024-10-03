using Domain;

namespace IBusinessLogic;

public interface ISessionService
{
    User GetUserByToken(string token);
    void AddSession(Session session);
    Session Authenticate(string email, string password);
}
