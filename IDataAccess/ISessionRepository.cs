using Domain;

namespace IDataAccess;

public interface ISessionRepository
{
    public User FindByMail(string mail); 
    public User FindByToken(Guid token);
    public void AddSession(Session session);
}
