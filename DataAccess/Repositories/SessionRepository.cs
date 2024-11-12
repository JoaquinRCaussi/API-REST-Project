using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using DataAccess.Data;

namespace DataAccess.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly HMDbContext _context;

    public SessionRepository(HMDbContext context)
    {
        _context = context;
    }

    public void AddSession(Session session)
    {
        _context.Sessions?.Add(session);
        _context.SaveChanges();
    }

    public Session? FindByToken(Guid? token)
    {
        var sessions = _context.Sessions;

        if (sessions == null)
        {
            return null;
        }

        return sessions.FirstOrDefault(s => s.Token == token);
    }

    public void RemoveSession(Session session)
    {
        _context.Sessions.Remove(session);
        _context.SaveChanges();
    }
}
