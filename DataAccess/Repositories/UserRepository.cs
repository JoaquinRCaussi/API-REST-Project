using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HMDbContext _context;

    public UserRepository(HMDbContext context)
    {
        _context = context;
    }

    public User CreateAdmin(User user)
    {
        Role? adminRole = _context.Roles?.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole != null)
        {
            user.RoleID = adminRole.Id;
            user.Role = adminRole;
        }

        _context.Users?.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User CreateCompanyOwner(User user)
    {
        Role? companyOwnerRole = _context.Roles?.FirstOrDefault(r => r.Name == "CompanyOwner");
        if (companyOwnerRole != null)
        {
            user.RoleID = companyOwnerRole.Id;
            user.Role = companyOwnerRole;
        }

        _context.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User CreateHomeOwner(User user)
    {
        Role? homeOwnerRole = _context.Roles?.FirstOrDefault(r => r.Name == "HomeOwner");
        if (homeOwnerRole != null)
        {
            user.RoleID = homeOwnerRole.Id;
            user.Role = homeOwnerRole;
        }

        _context.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User AddCompanyToCompanyOwner(User user, Company company)
    {
        user.CompanyID = company.Id;
        _context.SaveChanges();
        return user;
    }

    public List<User> GetUsers()
    {
        return _context.Set<User>()
            .Include(u => u.Role)
            .Include(u => u.Company)
            .ToList();
    }

    public User GetUser(Guid userId)
    {
        User? user = _context.Users?
            .Include(u => u.Role)
            .Include(u => u.Company)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        return user;
    }

    public User FindByMail(string mail)
    {
        User? user = _context.Users?
            .Include(u => u.Company)
            .Include(u => u.Role)
            .ThenInclude(r => r.PermissionKeys)
            .FirstOrDefault(u => u.Email == mail);

        if (user == null)
        {
            return null;
        }

        return user;
    }

    public User AuthenticateUser(string mail, string password)
    {
        User? user = _context.Users?.FirstOrDefault(u => u.Email == mail && u.Password == password);

        if (user == null)
        {
            return null;
        }

        return user;
    }

    public bool ExistUser(Guid userId)
    {
        User? user = _context.Users?.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        return true;
    }

    public User DeleteUser(Guid userId)
    {
        User? user = _context.Users?.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        _context.Users?.Remove(user);
        _context.SaveChanges();
        return user;
    }

    public List<Notification> GetNotifications(Guid userId)
    {
        var notifications = _context.Notifications?
            .Include(n => n.User)
            .Include(n => n.HomeDevice)
            .Where(n => n.UserId == userId)
            .ToList();

        if (notifications == null || notifications.Count == 0)
        {
            return [];
        }

        return notifications;
    }

    public List<User> GetUsersFiltered(string? role, string? fullName)
    {
        var users = _context.Users?
            .Include(u => u.Role)
            .Include(u => u.Company)
            .Where(u => (role == null || u.Role.Name == role) && (fullName == null || u.Name.Contains(fullName) || u.LastName.Contains(fullName)))
            .ToList();

        if (users == null || users.Count == 0)
        {
            return [];
        }

        return users;
    }
}
