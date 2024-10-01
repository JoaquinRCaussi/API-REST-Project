using DataAccess.Data;
using Domain;
using IDataAccess;

namespace DataAccess.Repositories;
public class SessionRepository : ISessionRepository
{
    private readonly List<Session> _sessions = [];
    private readonly HMDbContext _context;

    public SessionRepository(HMDbContext context)
    {
        _context = context;
    }

    public void AddSession(Session session)
    {
        _sessions.Add(session);
    }

    public Session? FindByToken(Guid token)
    {
        return _sessions.FirstOrDefault(s => s.Token == token);
    }
}
