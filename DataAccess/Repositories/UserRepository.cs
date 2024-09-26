using DataAccess.Data;
using Domain;
using IDataAccess;

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
            user.Role = adminRole.Id;
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
            user.Role = companyOwnerRole.Id;
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
            user.Role = homeOwnerRole.Id;
        }

        _context.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User AddCompanyToCompanyOwner(User user, Company company)
    {
        user.Company = company.Id;
        _context.SaveChanges();
        return user;
    }

    public List<User> GetUsers()
    {
        return _context.Set<User>().ToList();
    }

    public User GetUser(Guid userId)
    {
        var user = _context.Users?.FirstOrDefault(u => u.Id == userId);
        
        if (user == null)
        {
            return null;
        }

        return user;
    }
    
}
