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
        var homeWithMember = AddMember(homeResult.Id, homeResult.HomeOwner);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (homeWithMember == null)
        {
            return homeResult;
        }
        return homeWithMember;
    }

    public List<Home> GetHomes()
    {
        var result = _homeRepository.GetHomes();
        
        if (result.Count == 0)
        {
            throw new EmptyException("No homes found.");
        }
        
        return result;
    }

    public List<Home> GetHomesByUser(Guid userId)
    {
        var result = _homeRepository.GetHomesByUser(userId);
        
        if (result.Count == 0)
        {
            throw new EmptyException("No homes found for this user.");
        }
        
        return result;
    }

    public Home GetHome(Guid homeId)
    {
        var result = _homeRepository.GetHome(homeId);
        
        if (result == null)
        {
            throw new NotValidDataException("Home not found.");
        }
        
        return result;
    }

    public List<User> GetHomeMembers(Guid homeId)
    {
        var result = _homeRepository.GetHomeMembers(homeId);
        
        if (result.Count == 0)
        {
            throw new EmptyException("No members found.");
        }
        
        return result;
    }

    public Home AddMember(Guid homeId, Guid userId)
    {
        var home = _homeRepository.GetHome(homeId);
        var members = _homeRepository.GetHomeMembers(homeId);
        if (home != null && members.Count >= home.MemberCount)
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
            throw new NotValidDataException("Permission value is required");
        }

        var home = _homeRepository.GetHome(homeId);

        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        if (permissions.Enable)
        {
            _memberSettingRepository.AddPermission(homeId, userId, value);
        }
        else
        {
            _memberSettingRepository.RemovePermission(homeId, userId, value);
        }

        return home;
    }

    public HomeDevice AddDevice(Guid homeId, Guid deviceId)
    {
        return _homeRepository.AddDevice(homeId, deviceId);
    }

    public List<HomeDevice> GetHomeDevices(Guid homeId)
    {
        var result = _homeRepository.GetHomeDevices(homeId);
        
        if (result.Count == 0)
        {
            throw new EmptyException("No devices found.");
        }
        
        return result;
    }

    public List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, SensorRequest sensor)
    {
        if (sensor.Event != "open" && sensor.Event != "close")
        {
            throw new NotValidDataException("Event must be open or close");
        }

        if (GetHome(homeId) == null)
        {
            throw new NotValidDataException("Home not found");
        }

        var homeDevice = GetHomeDevices(homeId).Find(h => h.HardwareId == hardwareId);
        if (homeDevice == null)
        {
            throw new NotValidDataException("Device not found");
        }

        if (homeDevice.Device?.DeviceType != DeviceType.Sensor)
        {
            throw new NotValidDataException("Device is not a sensor");
        }

        _homeRepository.ChangeHomeDeviceStatus(homeId, hardwareId, sensor.Event == "open");

        return _notificationRepository.CreateNotificationSensor(homeId, hardwareId, sensor);
    }

    public List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, SensorRequest sensor)
    {
        if (sensor.Event != "movement-detected" && sensor.Event != "person-detected")
        {
            throw new NotValidDataException("Event must be movement-detected or person-detected");
        }

        if (GetHome(homeId) == null)
        {
            throw new NotValidDataException("Home not found");
        }

        var homeDevice = GetHomeDevices(homeId).Find(h => h.HardwareId == hardwareId);
        if (homeDevice == null)
        {
            throw new NotValidDataException("Device not found");
        }

        if (homeDevice.Device?.DeviceType != DeviceType.Camera)
        {
            throw new NotValidDataException("Device is not a camera");
        }

        return _notificationRepository.CreateNotificationCamera(homeId, hardwareId, sensor);
    }
}
