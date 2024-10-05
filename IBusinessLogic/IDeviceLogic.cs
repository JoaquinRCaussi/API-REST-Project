using Domain;

namespace IBusinessLogic;

public interface IDeviceLogic
{
    public Device CreateDevice(Device device);
    public Camera CreateCamera(Camera camera);
    public List<Device> GetDevices(string? name, string? companyName, DeviceType? deviceType);
    public List<string> GetDevicesTypes();
}
