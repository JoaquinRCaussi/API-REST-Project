using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface ISessionRepository
{
    public Session? FindByToken(Guid token);
    public void AddSession(Session session);
}
