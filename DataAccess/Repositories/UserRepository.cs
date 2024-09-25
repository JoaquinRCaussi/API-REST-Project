using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;
    
    public UserRepository(DbContext context)
    {
        _context = context;
    }
    
    public User CreateAdmin(User user)
    {
        _context.Add(user);
        _context.SaveChanges();
        return user;
    }
    
    public User CreateCompanyOwner(User user)
    {
        _context.Add(user);
        _context.SaveChanges();
        return user;
    }
    
    public User AddCompanyToCompanyOwner(User user, Company company)
    {
        user.Company = company;
        _context.SaveChanges();
        return user;
    }
    
    public List<User> GetUsers()
    {
        return _context.Set<User>().ToList();
    }
    
    
}
