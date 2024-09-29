using Domain;
using IDataAccess;
using LogicInterface;


namespace BusinessLogic;

public class DeviceLogic : IDeviceLogic
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceLogic(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public Device CreateDevice(Device device)
    {
        return _deviceRepository.CreateDevice(device);
    }

    public List<Device> GetDevices()  
    {
        return _deviceRepository.GetDevices();
    }
}
