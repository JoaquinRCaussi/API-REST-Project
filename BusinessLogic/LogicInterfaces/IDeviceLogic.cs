using BusinessLogic.Entities;

namespace BusinessLogic.LogicInterfaces;

public interface IDeviceLogic
{
    public Device CreateDevice(Device device);
    public Camera CreateCamera(Camera camera);
    (List<Device> Devices, int TotalResults) GetDevices(string? name, string? model, string? companyName, DeviceType? deviceType, int pageNumber = 1, int pageSize = 10);
    public List<string> GetDevicesTypes();
}
