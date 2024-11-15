using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface IDeviceLogic
{
    public Device CreateDevice(Device device);
    public Camera CreateCamera(Camera camera);
    public List<Device> GetDevices(string? name, string? model, string? companyName, DeviceType? deviceType);
    public List<string> GetDevicesTypes();
}
