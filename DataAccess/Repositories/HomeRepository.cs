using DataAccess.Data;
using Domain;
using IDataAccess;

namespace DataAccess.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly HMDbContext _dbContext;
    
    public HomeRepository(HMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Home CreateHome(Home home)
    {
        _dbContext.Homes?.Add(home);
        _dbContext.SaveChanges();
        return home;
    }
    
    public List<Home> GetHomes()
    {
        return _dbContext.Homes?.ToList()!;
    }
    
    public List<Home> GetHomesByUser(Guid userId)
    {
        return _dbContext.Homes?.Where(x => x.HomeOwner == userId).ToList()!;
    }
    
    public Home GetHome(Guid homeId)
    {
        return _dbContext.Homes?.FirstOrDefault(x => x.Id == homeId)!;
    }
    
    public List<User> GetHomeMembers(Guid homeId)
    {
        var members = _dbContext.Homes?
            .Where(x => x.Id == homeId)
            .Select(x => x.Members)
            .FirstOrDefault();
        
        if (members == null || members.Count == 0)
        {
            return new();
        }
        
        var users = _dbContext.Users?
            .Where(x => members.Contains(x.Id))
            .ToList();

        return users ?? new();
    }
    
    public Home AddMember(Guid homeId, Guid userId)
    {
        var home = _dbContext.Homes?.FirstOrDefault(x => x.Id == homeId);
        if (home == null)
        {
            return new()
            {
                Location = null,
                MemberCount = 0,
                Devices = null,
                HomeOwner = default
            };
        }
        
        home.Members?.Add(userId);
        _dbContext.SaveChanges();
        return home;
    }
}
