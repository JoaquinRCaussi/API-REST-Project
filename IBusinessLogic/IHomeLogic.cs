using Domain;
using Models;

namespace IBusinessLogic;

public interface IHomeLogic
{
    Home CreateHome(Home home);

    List<Home> GetHomes();

    List<Home> GetHomesByUser(Guid userId);

    Home GetHome(Guid homeId);

    List<User> GetHomeMembers(Guid homeId);

    Home AddMember(Guid homeId, Guid userId);

    Home UpdatePermissions(Guid homeId, Guid userId, PermissionRequest permission);

    HomeDevice AddDevice(Guid homeId, Guid deviceId);

    HomeDevice ChangeHomeDeviceName(Guid homeId, Guid hardwareId, string name);

    List<HomeDevice> GetHomeDevices(Guid homeId);

    List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, SensorRequest sensorRequest);

    List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, SensorRequest sensorRequest);
    
    Room AddRoom(Guid homeId, string name);
}
