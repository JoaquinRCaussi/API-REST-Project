using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface ISessionService
{
    User GetUserByToken(Guid token);
    void AddSession(Session session);
    Session Authenticate(string email, string password);
    void Logout(Guid token);
}
