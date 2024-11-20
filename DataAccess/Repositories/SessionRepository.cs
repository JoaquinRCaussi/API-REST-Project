using BusinessLogic.DataAccessInterfaces;
using BusinessLogic.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

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

        var filteredSessions = sessions.Where(s => s.Token == token)
                .Include(s => s.User)
                .Include(c => c.User.Company)
                .Include(s => s.User.Role)
                    .ThenInclude(r => r.PermissionKeys)
            .ToList();
        return filteredSessions.FirstOrDefault();
    }

    public void RemoveSession(Session session)
    {
        _context.Sessions.Remove(session);
        _context.SaveChanges();
    }
}
