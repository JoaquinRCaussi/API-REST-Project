using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface IDeviceRepository
{
    public Device CreateDevice(Device device);
    public Camera CreateCamera(Camera camera);
    public List<Device> GetDevices(string name, string model, string companyName, DeviceType deviceType);
    public bool ExistsDevice(string? name, Guid companyId);
    List<Device> GetDevicesNoType(string name, string model, string companyName);
}
