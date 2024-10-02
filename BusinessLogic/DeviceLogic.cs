using Domain;
using IDataAccess;
using IBusinessLogic;


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
    public Camera CreateCamera(Camera camera)
    {
        return (Camera)_deviceRepository.CreateDevice(camera);
    }

}
