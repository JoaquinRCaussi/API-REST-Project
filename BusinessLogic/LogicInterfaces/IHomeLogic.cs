using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface IHomeLogic
{
    Home CreateHome(Home home);

    Home ChangeHomeName(Guid homeId, string name);

    List<Home> GetHomes();

    List<Home> GetHomesByUser(Guid userId);

    Home GetHome(Guid homeId);

    List<User> GetHomeMembers(Guid homeId);

    Home AddMember(Guid homeId, Guid userId);

    Home UpdatePermissions(Guid homeId, Guid userId, string permission, bool addPermission);

    HomeDevice AddDevice(Guid homeId, Guid deviceId);

    HomeDevice ChangeHomeDeviceName(Guid homeId, Guid hardwareId, string name);

    List<HomeDevice> GetHomeDevices(Guid homeId, Guid? roomId = null);

    List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, string anEvent);

    List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, string anEvent);

    Room AddRoom(Guid homeId, string name);

    List<Room> GetRooms(Guid homeId);

    Room AddDeviceToRoom(Guid homeId, Guid? hardwareId, Guid roomId);
}
