using Domain;

namespace IDataAccess;

public interface IDeviceRepository
{
    public Device CreateDevice(Device device);
    List<Device> GetDevices();
}
