using Domain;

namespace IDataAccess;

public interface IHomeRepository
{
    public Home CreateHome(Home home);

    public List<Home> GetHomes();

    public List<Home> GetHomesByUser(Guid userId);

    public Home GetHome(Guid homeId);

    public List<User> GetHomeMembers(Guid homeId);

    public Home AddMember(Guid homeId, Guid userId);

    public HomeDevice AddDevice(Guid homeId, Guid deviceId);

    public List<HomeDevice> GetHomeDevices(Guid homeId, Guid? roomId = null);

    public HomeDevice ChangeHomeDeviceStatus(Guid homeId, Guid hardwareId, bool state);

    public HomeDevice ChangeHomeDeviceName(Guid homeId, Guid hardwareId, string name);

    public Room AddRoom(Guid homeId, string name);

    public List<Room> GetRooms(Guid homeId);

    public Room AddDeviceToRoom(Guid homeId, Guid? hardwareId, Guid roomId);
}
