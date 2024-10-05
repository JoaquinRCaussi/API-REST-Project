using Domain;
using IBusinessLogic;
using IDataAccess;


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
        if (_deviceRepository.ExistsDevice(device.Name, device.Company.Id))
        {
            throw new ConflictException("The Device already exists");
        }
        return _deviceRepository.CreateDevice(device);
    }
    public Camera CreateCamera(Camera camera)
    {
        if (_deviceRepository.ExistsDevice(camera.Name, camera.Company.Id))
        {
            throw new ConflictException("The Device already exists");
        }
        return _deviceRepository.CreateCamera(camera);
    }
    

    public List<Device> GetDevices(string? name, string? model, string? companyName, DeviceType? deviceType)
    {
        List<Device> result = new List<Device>();
        if (name == null)
        {
            name = "";
        }
        if (companyName == null)
        {
            companyName = "";
        }

        if (model == null)
        {
            model = "";
        }
        if (deviceType == null)
        {
            result = _deviceRepository.GetDevicesNoType(name, model, companyName);
        }
        else
        {
            result = _deviceRepository.GetDevices(name,model, companyName, (DeviceType)deviceType);
        }
        return result;
    }
    
    public List<string> GetDevicesTypes()
    {
        return Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().Select(x => x.ToString()).ToList();
    }
}
