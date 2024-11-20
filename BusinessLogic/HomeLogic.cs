using BusinessLogic.DataAccessInterfaces;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;

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

        if (homeResult == null)
        {
            throw new NotValidDataException("Home could not be created");
        }

        return homeResult;
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
            throw new EmptyException("No members found for this home.");
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

        var member = _homeRepository.AddMember(homeId, userId);

        var permissions = UpdatePermissions(homeId, userId, "CanGetNotifications", true);

        if (member == null || permissions == null)
        {
            throw new NotValidDataException("Member could not be added");
        }

        return home;
    }

    public Home UpdatePermissions(Guid homeId, Guid userId, string permissions, bool addPermission)
    {
        var value = permissions;

        if (value == null)
        {
            throw new NotValidDataException("Permission value is required");
        }

        var home = _homeRepository.GetHome(homeId);

        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        if (addPermission)
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

    public HomeDevice ChangeHomeDeviceName(Guid homeId, Guid hardwareId, string name)
    {
        var home = _homeRepository.GetHome(homeId);
        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        var homeDevice = _homeRepository.GetHomeDevices(homeId).Find(h => h.HardwareId == hardwareId);
        if (homeDevice == null)
        {
            throw new NotValidDataException("Device not found");
        }

        var result = _homeRepository.ChangeHomeDeviceName(homeId, hardwareId, name);

        if (result == null)
        {
            throw new NotValidDataException("Device name could not be changed");
        }

        return result;
    }

    public List<HomeDevice> GetHomeDevices(Guid homeId, Guid? roomId = null)
    {
        var result = _homeRepository.GetHomeDevices(homeId, roomId);

        if (result == null)
        {
            throw new NotValidDataException("Home not found.");
        }

        if (result.Count == 0)
        {
            if (roomId == null)
            {
                throw new EmptyException("No devices found for this home.");
            }
            else
            {
                throw new EmptyException("No devices found for this room in this home.");
            }
        }

        return result;
    }

    public List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, string anEvent)
    {
        if (anEvent != "open" && anEvent != "close")
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

        if (homeDevice.Device?.DeviceType != DeviceType.WindowSensor)
        {
            throw new NotValidDataException("Device is not a sensor");
        }

        _homeRepository.ChangeHomeDeviceStatus(homeId, hardwareId, anEvent == "open");

        return _notificationRepository.CreateNotificationSensor(homeId, hardwareId, anEvent);
    }

    public List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, string anEvent)
    {
        if (anEvent != "movement-detected" && anEvent != "person-detected")
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

        return _notificationRepository.CreateNotificationCamera(homeId, hardwareId, anEvent);
    }

    public Room AddRoom(Guid homeId, string name)
    {
        var home = _homeRepository.GetHome(homeId);

        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        return _homeRepository.AddRoom(homeId, name);
    }

    public List<Room> GetRooms(Guid homeId)
    {
        var home = _homeRepository.GetHome(homeId);

        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        var rooms = _homeRepository.GetRooms(homeId);

        if (rooms.Count == 0)
        {
            throw new EmptyException("No rooms found for this home.");
        }

        return rooms;
    }

    public Room AddDeviceToRoom(Guid homeId, Guid? hardwareId, Guid roomId)
    {
        var home = _homeRepository.GetHome(homeId);

        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        var room = _homeRepository.GetRooms(homeId).Find(r => r.Id == roomId);

        if (room == null)
        {
            throw new NotValidDataException("Room not found");
        }

        var homeDevice = _homeRepository.GetHomeDevices(homeId).Find(h => h.HardwareId == hardwareId);

        if (homeDevice == null)
        {
            throw new NotValidDataException("Device not found");
        }

        var roomd = _homeRepository.AddDeviceToRoom(homeId, hardwareId, roomId);

        if (roomd == null)
        {
            throw new NotValidDataException("Device could not be added to room");
        }

        return roomd;
    }

    public Home ChangeHomeName(Guid homeId, string name)
    {
        var home = _homeRepository.GetHome(homeId);

        if (home == null)
        {
            throw new NotValidDataException("Home not found");
        }

        var result = _homeRepository.ChangeHomeName(homeId, name);

        if (result == null)
        {
            throw new NotValidDataException("Home name could not be changed");
        }

        return result;
    }
}
