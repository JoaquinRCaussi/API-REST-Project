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
        _deviceRepository.Setup(x => x.GetDevices("Device", "", "")).Returns(devices);
        
        var deviceLogic = new DeviceLogic(_deviceRepository.Object);
        
        var result = deviceLogic.GetDevices("Device", "", "");
        
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

}
