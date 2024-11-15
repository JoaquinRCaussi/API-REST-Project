using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using BusinessLogic.Validators;
using ModeloValidador.Abstracciones;

namespace BusinessLogic;

public class DeviceLogic : IDeviceLogic
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ValidatorService _validatorService;

    public DeviceLogic(IDeviceRepository deviceRepository, ICompanyRepository companyRepository, ValidatorService validatorService)
    {
        _deviceRepository = deviceRepository;
        _companyRepository = companyRepository;
        _validatorService = validatorService;
    }

    public Device CreateDevice(Device device)
    {
        if (device.Company == null)
        {
            throw new NotValidDataException("The User must have a Company registered");
        }

        var company = device.Company;
        if (company.ValidatorModelName.Length > 0)
        {
            var validator = _validatorService.GetValidatorByName(company.ValidatorModelName);

            if (device.Model == null)
            {
                throw new NotValidDataException("The model is required");
            }

            var model = new Modelo()
            {
                Value = device.Model
            };

            if (!validator.EsValido(model))
            {
                throw new NotValidDataException("The model is not valid");
            }
        }
        else
        {
            throw new NotValidDataException("The company does not have a validator");
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

        var company = camera.Company;
        if (company.ValidatorModelName.Length > 0)
        {
            var validator = _validatorService.GetValidatorByName(company.ValidatorModelName);

            if (camera.Model == null)
            {
                throw new NotValidDataException("The model is required");
            }

            var model = new Modelo()
            {
                Value = camera.Model
            };

            if (!validator.EsValido(model))
            {
                throw new NotValidDataException("The model is not valid");
            }
        }
        else
        {
            throw new NotValidDataException("The company does not have a validator");
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


    public List<Device> GetDevices(DeviceFilterRequest filter)
    {
        // Validar si los campos son nulos y, de ser así, inicializarlos con valores por defecto.
        filter.Name ??= "";
        filter.Model ??= "";
        filter.CompanyName ??= "";

        // Llamar al repositorio con el objeto de filtro.
        var result = _deviceRepository.GetDevices(filter);

        // Verificar si se encontraron dispositivos.
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
