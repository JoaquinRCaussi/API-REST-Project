using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface IDeviceRepository
{
    public Device CreateDevice(Device device);
    public Camera CreateCamera(Camera camera);
    (List<Device> Devices, int TotalResults) GetDevices(string name, string model, string companyName, DeviceType? deviceType, int pageNumber, int pageSize); public bool ExistsDevice(string? name, Guid companyId);
}
