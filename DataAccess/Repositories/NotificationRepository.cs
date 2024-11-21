using BusinessLogic.DataAccessInterfaces;
using BusinessLogic.Entities;
using DataAccess.Data;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace DataAccess;

public class NotificationRepository : INotificationRepository
{
    private readonly HMDbContext _context;

    public NotificationRepository(HMDbContext context)
    {
        _context = context;
    }

    public List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, string anEvent)
    {
        var listOfNotifications = new List<Notification>();
        var home = _context.Homes?
            .Include(h => h.Devices)
            .Include(h => h.Members)
            .Include(h => h.MemberSettings)
                .ThenInclude(ms => ms.Permissions)
            .FirstOrDefault(x => x.Id == homeId);

        var homeDevice = home?.Devices?.FirstOrDefault(x => x.HardwareId == hardwareId);

        if (home == null || homeDevice == null)
        {
            throw new EntityNotFoundException("Home or device not found");
        }

        foreach (var member in home.MemberSettings)
        {
            var user = _context.Users?.FirstOrDefault(x => x.Id == member.UserId);
            if (user != null)
            {
                var hasPermission = member.Permissions
                    .Any(p => p.Value == "CanGetNotifications");

                if (hasPermission)
                {
                    var notification = new Notification
                    {
                        UserId = member.Id,
                        Event = anEvent,
                        CreatedAt = DateTime.Now,
                        HardwareId = hardwareId,
                        HomeDevice = homeDevice,
                        User = user,
                        IsRead = false
                    };

                    listOfNotifications.Add(notification);
                    _context.Notifications?.Add(notification);
                }
            }
        }
        _context.SaveChanges();
        return listOfNotifications;
    }

    public List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, string anEvent)
    {
        var listOfNotifications = new List<Notification>();
        var home = _context.Homes?
            .Include(h => h.Devices)
            .Include(h => h.Members)
            .Include(h => h.MemberSettings)
                .ThenInclude(ms => ms.Permissions)
            .FirstOrDefault(x => x.Id == homeId);

        var homeDevice = home?.Devices?.FirstOrDefault(x => x.HardwareId == hardwareId);

        if (home == null || homeDevice == null)
        {
            throw new Exception("Home or device not found");
        }

        foreach (var member in home.Members)
        {
            var memberSettings = home.MemberSettings?.FirstOrDefault(x => x.UserId == member.Id);
            if (memberSettings != null)
            {
                var hasPermission = memberSettings.Permissions
                    .Any(p => p.Value == "CanGetNotifications");

                if (hasPermission)
                {
                    var notification = new Notification
                    {
                        UserId = member.Id,
                        Event = anEvent,
                        CreatedAt = DateTime.Now,
                        HardwareId = hardwareId,
                        HomeDevice = homeDevice,
                        User = member,
                        IsRead = false
                    };

                    listOfNotifications.Add(notification);
                    _context.Notifications?.Add(notification);
                }
            }
        }
        _context.SaveChanges();
        return (listOfNotifications.IsNullOrEmpty() ? null : listOfNotifications) ?? throw new InvalidOperationException();
    }
}
