using Domain;

namespace IBusinessLogic;

public interface IDeviceLogic
{
    Device CreateDevice(Device device);
    Camera CreateCamera(Camera camera);
    List<Device> GetDevices(string? name, string? companyName, string? deviceType);
}
