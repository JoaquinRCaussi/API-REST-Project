using Domain;
using FluentAssertions;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class DevicesLogicTest
{
    private Mock<IDeviceRepository>? _deviceRepository;
    private User? _user;
    private Company? _company;
    private Device? _device;

    [TestInitialize]
    public void Setup()
    {
        _deviceRepository = new Mock<IDeviceRepository>();
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
            Owner = _user
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
            Photo = "Photo",
            Company = _company
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

        var result = deviceLogic.CreateDevice(_device);

        result.Should().BeEquivalentTo(_device);
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
            Photo = "Photo",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(false);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

        var result = deviceLogic.CreateCamera(camera);

        result.Should().BeEquivalentTo(camera);
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
        var devices = new List<Device>
        {
            device
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _deviceRepository.Setup(x => x.GetDevices("Device", "", "", It.IsAny<DeviceType>())).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

        var result = deviceLogic.GetDevices("Device", "","", DeviceType.Camera);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
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
            Photo = "Photo",
            Company = _company
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _deviceRepository.Setup(x => x.ExistsDevice(_device.Name, _device.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.CreateDevice(It.IsAny<Device>())).Returns(_device);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

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
            Photo = "Photo",
            Company = _company,
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _deviceRepository.Setup(x => x.ExistsDevice(camera.Name, camera.Company.Id)).Returns(true);
        _deviceRepository.Setup(x => x.CreateCamera(It.IsAny<Camera>())).Returns(camera);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

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
        _deviceRepository.Setup(x => x.GetDevices("", "","Company", DeviceType.Camera)).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

        var result = deviceLogic.GetDevices("", "","Company", DeviceType.Camera);

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
        _deviceRepository.Setup(x => x.GetDevices("", "","", DeviceType.Camera)).Returns(devices);

        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

        var result = deviceLogic.GetDevices("", "","", DeviceType.Camera);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetDevicesTypesOk()
    {
        var deviceTypes = new List<string>
        {
            "Camera",
            "Sensor"
        };
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        var deviceLogic = new DeviceLogic(_deviceRepository.Object);

        var result = deviceLogic.GetDevicesTypes();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(deviceTypes);
    }
}
