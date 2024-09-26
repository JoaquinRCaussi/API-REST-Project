using DataAccess.Data;
using Domain;
using IDataAccess;
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
        var adminRole = _context.Roles?.FirstOrDefault(r => r.Name == "Admin");
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
        var companyOwnerRole = _context.Roles?.FirstOrDefault(r => r.Name == "CompanyOwner");
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
        var homeOwnerRole = _context.Roles?.FirstOrDefault(r => r.Name == "HomeOwner");
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
        return _context.Users?.Include(u => u.Role).Include(u => u.Company).FirstOrDefault(u => u.Id == userId)!;
    }
}
