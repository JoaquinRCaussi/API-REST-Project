using System.Diagnostics.CodeAnalysis;
using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using BusinessLogic.Validators;
using FluentAssertions;
using ModeloValidador.Abstracciones;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class DevicesLogicTest
{
    private Mock<IDeviceRepository>? _deviceRepository;
    private Mock<ICompanyRepository>? _companyRepository;
    private Mock<ValidatorService>? _validatorService;
    private User? _user;
    private Company? _company;
    private Device? _device;

    [TestInitialize]
    public void Setup()
    {
        _deviceRepository = new Mock<IDeviceRepository>();
        _companyRepository = new Mock<ICompanyRepository>();
        _validatorService = new Mock<ValidatorService>();

        _user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Company",
            RUT = "Address",
            Logo = "Logo",
            Owner = _user,
            ValidatorModelName = "ModeloValidadorFormato3"
        };
    }

    [TestMethod]
    public void CreateDeviceTest_WhenAllPropertiesAreOk()
    {
        _device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(_device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(_device.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.CreateDevice(_device);

        result.Should().BeEquivalentTo(_device);
    }

    [TestMethod]
    public void CreateDeviceTest_WhenValidatorIsValid_ShouldCreateDevice()
    {
        _device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "ValidModel",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(_device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(_device.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.CreateDevice(_device);

        result.Should().BeEquivalentTo(_device);
    }

    [TestMethod]
    public void CreateDeviceTest_WhenValidatorIsInvalid_ShouldThrowException()
    {
        _device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "InvalidModel",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(_device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(_device.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(false);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateDevice(_device);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("The model is not valid");
    }

    [TestMethod]
    public void CreateDeviceTest_WhenCompanyHasNoValidator_ShouldThrowException()
    {
        _device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "ValidModel",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(_device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        _company.ValidatorModelName = string.Empty;

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateDevice(_device);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("The company does not have a validator");
    }

    [TestMethod]
    public void CreateDeviceTest_WhenModelIsNull_ShouldThrowException()
    {
        _device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = null,
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(_device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(_device.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateDevice(_device);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("The model is required");
    }

    [TestMethod]
    public void CreateCameraTest_WhenAllPropertiesAreOk()
    {
        var camera = new Camera
        {
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(camera.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.CreateCamera(camera);

        result.Should().BeEquivalentTo(camera);
    }

    [TestMethod]
    public void CreateCameraTest_WhenValidatorIsValid_ShouldCreateDevice()
    {
        var camera = new Camera
        {
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(camera.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.CreateCamera(camera);

        result.Should().BeEquivalentTo(camera);
    }

    [TestMethod]
    public void CreateCameraTest_WhenValidatorIsInvalid_ShouldThrowException()
    {
        var camera = new Camera
        {
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "InvalidModel",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(camera.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(false);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateCamera(camera);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("The model is not valid");
    }

    [TestMethod]
    public void CreateCameraTest_WhenCompanyHasNoValidator_ShouldThrowException()
    {
        var camera = new Camera
        {
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "InvalidModel",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        _company.ValidatorModelName = string.Empty;

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateCamera(camera);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("The company does not have a validator");
    }

    [TestMethod]
    public void CreateCameraTest_WhenModelIsNull_ShouldThrowException()
    {
        var camera = new Camera
        {
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = null,
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _companyRepository.Setup(x => x.ExistsCompany(camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(camera.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateCamera(camera);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("The model is required");
    }

    [TestMethod]
    public void GetDevicesTest_WhenFilterByName()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo",
            Company = _company
        };
        var devices = new List<Device> { device };
        var totalResults = devices.Count;

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _deviceRepository.Setup(x => x.GetDevices("Device", "", "", DeviceType.Camera, 1, 10))
                         .Returns((devices, totalResults));

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var (result, count) = deviceLogic.GetDevices("Device", "", "", DeviceType.Camera, 1, 10);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        count.Should().Be(1);
    }

    [TestMethod]
    public void GetDevices_WhenNoDevicesFoundShouldThrowException()
    {
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _deviceRepository.Setup(x => x.GetDevices("", "", "", DeviceType.Camera)).Returns([]);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.GetDevices("", "", "", DeviceType.Camera);

        act.Should().Throw<EmptyException>().WithMessage("No devices found.");
    }

    [TestMethod]
    public void CreateDevice_WhenCompanyHasTheSameAlreadyCreatedShouldThrowException()
    {
        _device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(_device.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateDevice(_device);

        act.Should().Throw<ConflictException>().WithMessage("The Device already exists");
    }

    [TestMethod]
    public void CreateCamera_WhenCompanyHasTheSameAlreadyCreatedShouldThrowException()
    {
        var camera = new Camera
        {
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo.png",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };

        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var mockValidator = new Mock<IModeloValidador>();
        _validatorService.Setup(v => v.GetValidatorByName(camera.Company.ValidatorModelName))
            .Returns(mockValidator.Object);

        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        Action act = () => deviceLogic.CreateCamera(camera);

        act.Should().Throw<ConflictException>().WithMessage("The Device already exists");
    }


    [TestMethod]
    public void GetDevicesTest_WhenFilterByCompanyName()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo",
            Company = _company
        };
        var devices = new List<Device>
        {
            device
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _deviceRepository.Setup(x => x.GetDevices("", "", "Company", DeviceType.Camera)).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.GetDevices("", "", "Company", DeviceType.Camera);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetDevicesTest_WhenFilterByDeviceType()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo",
            Company = _company
        };
        var devices = new List<Device>
        {
            device
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        _deviceRepository.Setup(x => x.GetDevices("", "", "", DeviceType.Camera)).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.GetDevices("", "", "", DeviceType.Camera);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetDevicesTypesOk()
    {
        var deviceTypes = new List<string>
        {
            "Camera",
            "WindowSensor",
            "MovementSensor",
            "SmartLamp"
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _validatorService = new Mock<ValidatorService>(MockBehavior.Strict);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.GetDevicesTypes();

        result.Should().NotBeNull();
        result.Should().HaveCount(4);
        result.Should().BeEquivalentTo(deviceTypes);
    }

    [TestMethod]
    public void GetDevicesTest_WhenAllParametersAreNull()
    {
        var devices = new List<Device>
        {
            new Device { Id = Guid.NewGuid(), Name = "Device1", Model = "Model1", DeviceType = DeviceType.Camera, Company = _company },
            new Device { Id = Guid.NewGuid(), Name = "Device2", Model = "Model2", DeviceType = DeviceType.Camera, Company = _company }
        };

        _deviceRepository.Setup(x => x.GetDevicesNoType("", "", "")).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.GetDevices(null, null, null, null);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void GetDevicesTest_WhenOnlyDeviceTypeIsProvided()
    {
        var devices = new List<Device>
        {
            new Device { Id = Guid.NewGuid(), Name = "Device1", Model = "Model1", DeviceType = DeviceType.Camera, Company = _company }
        };

        _deviceRepository.Setup(x => x.GetDevices("", "", "", DeviceType.Camera)).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.GetDevices(null, null, null, DeviceType.Camera);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void GetDevicesTest_WhenFilterByModelAndCompanyName()
    {
        var devices = new List<Device>
    {
        new Device { Id = Guid.NewGuid(), Name = "Device1", Model = "Model1", DeviceType = DeviceType.Camera, Company = _company }
    };

        _deviceRepository.Setup(x => x.GetDevices("", "Model1", "Company", It.IsAny<DeviceType>())).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object, _companyRepository.Object, _validatorService.Object);

        var result = deviceLogic.GetDevices(null, "Model1", "Company", DeviceType.Camera);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(devices);
    }
}
