using DataAccess.Data;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

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
            return [];
        }

        var users = _dbContext.Users?
            .Where(x => members.Contains(x))
            .ToList();

        return users ?? [];
    }

    public Home AddMember(Guid homeId, Guid userId)
    {
        var home = _dbContext.Homes?.FirstOrDefault(x => x.Id == homeId);
        var user = _dbContext.Users?.FirstOrDefault(x => x.Id == userId);

        if (home == null || user == null)
        {
            return new()
            {
                Location = null,
                MemberCount = 0,
                Devices = null,
                HomeOwner = default
            };
        }

        home.Members?.Add(user);
        _dbContext.SaveChanges();
        return home;
    }
    
    public Home AddDevice(Guid homeId, Guid deviceId)
    {
        
        var home = _dbContext.Homes?.FirstOrDefault(x => x.Id == homeId);
        var device = _dbContext.Devices?.FirstOrDefault(x => x.Id == deviceId);

        if (home == null || device == null)
        {
            return new()
            {
                Location = null,
                MemberCount = 0,
                Devices = null,
                HomeOwner = default
            };
        }
        var homeDevice = new HomeDevice
        {
            DeviceId = deviceId,
            Device = device
        };
        
        _dbContext.HomeDevices?.Add(homeDevice);
        home.Devices?.Add(homeDevice);
        _dbContext.SaveChanges();
        return home;
    }
    
    public List<HomeDevice> GetHomeDevices(Guid homeId)
    {
        var home = _dbContext.Homes?
            .Include(h => h.Devices) // Carga los dispositivos asociados a la Home
            .FirstOrDefault(x => x.Id == homeId);

        return home.Devices;
    }
}
