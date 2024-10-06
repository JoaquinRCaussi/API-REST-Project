using Domain;
using IBusinessLogic;
using IDataAccess;
using Models;

namespace BusinessLogic;

public class HomeLogic : IHomeLogic
{
    private readonly IHomeRepository _homeRepository;
    private readonly IMemberSettingRepository _memberSettingRepository;
    private readonly INotificationRepository _notificationRepository;

    public HomeLogic(IHomeRepository homeRepository, IMemberSettingRepository memberSettingRepository, INotificationRepository notificationRepository)
    {
        _homeRepository = homeRepository;
        _memberSettingRepository = memberSettingRepository;
        _notificationRepository = notificationRepository;
    }
    public Home CreateHome(Home home)
    {
        var homeResult = _homeRepository.CreateHome(home);
        var homeWithMember = _homeRepository.AddMember(homeResult.Id, homeResult.HomeOwner);
        if (homeWithMember == null)
        {
            return homeResult;
        }
        return homeWithMember;
    }

    public List<Home> GetHomes()
    {
        return _homeRepository.GetHomes();
    }

    public List<Home> GetHomesByUser(Guid userId)
    {
        return _homeRepository.GetHomesByUser(userId);
    }

    public Home GetHome(Guid homeId)
    {
        return _homeRepository.GetHome(homeId);
    }

    public List<User> GetHomeMembers(Guid homeId)
    {
        return _homeRepository.GetHomeMembers(homeId);
    }

    public Home AddMember(Guid homeId, Guid userId)
    {
        var home = _homeRepository.GetHome(homeId);
        var members = _homeRepository.GetHomeMembers(homeId);
        if (members.Count >= home.MemberCount)
        {
            throw new ConflictException("House is full. Member limit has been reached.");
        }
        return _homeRepository.AddMember(homeId, userId);
    }

    public Home UpdatePermissions(Guid homeId, Guid userId, PermissionRequest permissions)
    {
        var value = permissions.Value;

        if (value == null)
        {
            throw new ArgumentNullException("Permission value is required");
        }

        if (permissions.Enable)
        {
            _memberSettingRepository.AddPermission(homeId, userId, value);
        }
        else
        {
            _memberSettingRepository.RemovePermission(homeId, userId, value);
        }

        return _homeRepository.GetHome(homeId);
    }

    public HomeDevice AddDevice(Guid homeId, Guid deviceId)
    {
        return _homeRepository.AddDevice(homeId, deviceId);
    }

    public List<HomeDevice> GetHomeDevices(Guid homeId)
    {
        return _homeRepository.GetHomeDevices(homeId);
    }

    public List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, SensorRequest sensor)
    {
        return _notificationRepository.CreateNotificationSensor(homeId, hardwareId, sensor);
    }

    public List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, SensorRequest sensor)
    {
        return _notificationRepository.CreateNotificationCamera(homeId, hardwareId, sensor);
    }
}
