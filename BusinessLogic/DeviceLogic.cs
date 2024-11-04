using Domain;
using IBusinessLogic;
using IDataAccess;


namespace BusinessLogic;

public class DeviceLogic : IDeviceLogic
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ICompanyRepository _companyRepository;

    public DeviceLogic(IDeviceRepository deviceRepository, ICompanyRepository companyRepository)
    {
        _deviceRepository = deviceRepository;
        _companyRepository = companyRepository;
    }
    public Device CreateDevice(Device device)
    {
        if (device.Company == null)
        {
            throw new NotValidDataException("The User must have a Company registered");
        }

        if (!IsCorrectImagePath(device.Photo))
        {
            throw new NotValidDataException("Image path must be one of these (.jpg, .jpeg, .png, .gif).");
        }

        if (_deviceRepository.ExistsDevice(device.Name, device.Company.Id))
        {
            throw new ConflictException("The Device already exists");
        }

        if (!_companyRepository.ExistsCompany(device.Company.Id))
        {
            throw new NotValidDataException("The Company does not exist");
        }

        return _deviceRepository.CreateDevice(device);
    }
    public Camera CreateCamera(Camera camera)
    {
        if (camera.Company == null)
        {
            throw new NotValidDataException("The User must have a Company registered");
        }

        if (!IsCorrectImagePath(camera.Photo))
        {
            throw new NotValidDataException("Image path must be one of these (.jpg, .jpeg, .png, .gif).");
        }
        if (_deviceRepository.ExistsDevice(camera.Name, camera.Company.Id))
        {
            throw new ConflictException("The Device already exists");
        }
        return _deviceRepository.CreateCamera(camera);
    }


    public List<Device> GetDevices(string? name, string? model, string? companyName, DeviceType? deviceType)
    {
        _ = new List<Device>();
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

        List<Device> result;
        if (deviceType == null)
        {
            result = _deviceRepository.GetDevicesNoType(name, model, companyName);
        }
        else
        {
            result = _deviceRepository.GetDevices(name, model, companyName, (DeviceType)deviceType);
        }

        if (result.Count == 0)
        {
            throw new EmptyException("No devices found.");
        }

        return result;
    }

    public List<string> GetDevicesTypes()
    {
        return Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().Select(x => x.ToString()).ToList();
    }
    public bool IsCorrectImagePath(string imagePath)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        var fileExtension = Path.GetExtension(imagePath).ToLower();

        return validExtensions.Contains(fileExtension);
    }
}
