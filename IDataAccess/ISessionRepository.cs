using Domain;

namespace IDataAccess;

public interface ISessionRepository
{
    public Session? FindByToken(Guid token);
    public void AddSession(Session session);
}
