using BusinessLogic.Entities;

namespace BusinessLogic.DataAccessInterfaces;

public interface ISessionRepository
{
    public Session? FindByToken(Guid? token);
    public void AddSession(Session session);
    public void RemoveSession(Session session);
}
