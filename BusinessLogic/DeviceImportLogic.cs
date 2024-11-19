using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using ImporterInterface;
using System.Reflection;

namespace BusinessLogic;
public class DeviceImportLogic : IDeviceImportLogic
{
    private readonly IDeviceLogic _deviceLogic;
    private readonly ICompanyLogic _companyLogic;

    public DeviceImportLogic(IDeviceLogic deviceLogic, ICompanyLogic companyLogic)
    {
        _deviceLogic = deviceLogic;
        _companyLogic = companyLogic;
    }

    private Device CreateDevice(string companyName, DeviceDTO deviceDto)
    {
        var company = _companyLogic.GetCompanies(companyName, null, 1, 1).Companies.FirstOrDefault();
        return new Device
        {
            CompanyId = company.Id,
            Company = company,
            Name = deviceDto.Nombre,
            Model = deviceDto.Modelo,
            DeviceType = deviceDto.Tipo switch
            {
                "camera" => DeviceType.Camera,
                "sensor-movement" => DeviceType.MovementSensor,
                "sensor-open-close" => DeviceType.WindowSensor,
                _ => DeviceType.SmartLamp
            },
            Description = string.Empty,
            Photo = deviceDto.Fotos.FirstOrDefault() ?? string.Empty,
        };

    }

    private Camera CreateCamera(string companyName, DeviceDTO deviceDto)
    {
        var company = _companyLogic.GetCompanies(companyName, null, 1, 1).Companies.FirstOrDefault();
        return new Camera
        {
            CompanyId = company.Id,
            Company = company,
            Name = deviceDto.Nombre,
            Model = deviceDto.Modelo,
            DeviceType = DeviceType.Camera,
            Description = string.Empty,
            Photo = deviceDto.Fotos.FirstOrDefault() ?? string.Empty,
            Outdoors = false,
            Indoors = false,
            SupportMovementDetection = deviceDto.MovementDetection ?? false,
            SupportPersonDetection = deviceDto.PersonDetection ?? false
        };
    }

    public void ImportDevices(string companyName, string assemblyPath)
    {
        var importer = LoadImporter(assemblyPath);
        var request = importer.ImportDevices();

        foreach (var deviceDto in request)
        {
            try
            {
                if (deviceDto.Tipo == "camera")
                {
                    Camera camera = CreateCamera(companyName, deviceDto);
                    _deviceLogic.CreateCamera(camera);
                }
                else
                {
                    Device device = CreateDevice(companyName, deviceDto);
                    _deviceLogic.CreateDevice(device);
                }
            }
            catch (ConflictException ex)
            {
                Console.WriteLine($"Conflicto al crear el dispositivo {deviceDto.Nombre}: {ex.Message}");
            }
        }
    }

    private IDeviceImporter LoadImporter(string assemblyPath)
    {
        var assembly = Assembly.LoadFrom(assemblyPath);

        foreach (Type type in assembly.GetTypes())
        {
            if (typeof(IDeviceImporter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            {
                return (IDeviceImporter)Activator.CreateInstance(type);
            }
        }

        throw new InvalidOperationException("No valid importer found in assembly.");
    }

}
