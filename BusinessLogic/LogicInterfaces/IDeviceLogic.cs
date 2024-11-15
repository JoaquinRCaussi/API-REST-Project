using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface IDeviceLogic
{
    public Device CreateDevice(Device device);
    public Camera CreateCamera(Camera camera);
    List<Device> GetDevices(DeviceFilterRequest filter);
    public List<string> GetDevicesTypes();
}
