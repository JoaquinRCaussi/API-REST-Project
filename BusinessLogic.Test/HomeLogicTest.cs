using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Models;
using Moq;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeLogicTest
{
    private Mock<IHomeRepository>? _homeRepositoryMock;
    private Mock<IMemberSettingRepository>? _memberSettingRepositoryMock;
    private Mock<INotificationRepository>? _notificationRepositoryMock;
    private Mock<IUserRepository>? _userRepositoryMock;
    private IHomeLogic? _homeLogic;
    private Company? _company;

    [TestInitialize]
    public void Initialize()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Matias",
            LastName = "Cabrera",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };
        _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Owner = user
        };
        _homeRepositoryMock = new Mock<IHomeRepository>();
        _memberSettingRepositoryMock = new Mock<IMemberSettingRepository>();
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _homeLogic = new HomeLogic(_homeRepositoryMock.Object, _memberSettingRepositoryMock.Object, _notificationRepositoryMock.Object);
    }

    [TestMethod]
    public void GetHomesTest()
    {

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };

        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };

        _userRepositoryMock?.Setup(x => x.GetUser(user.Id)).Returns(user);

        var homes = new List<Home>
        {
            new Home
            {
            Id = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [user],
            MemberCount = 5,
            Devices = homeDevices
        }
        };

        _homeRepositoryMock?.Setup(x => x.GetHomes()).Returns(homes);

        var result = _homeLogic?.GetHomes();

        result.Should().BeEquivalentTo(homes);
    }

    [TestMethod]
    public void CreateHomeTest()
    {
        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };

        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            MemberCount = 5,
            Devices = homeDevices
        };

        _userRepositoryMock?.Setup(x => x.GetUser(user.Id)).Returns(user);
        _homeRepositoryMock?.Setup(x => x.CreateHome(home)).Returns(home);

        var result = _homeLogic?.CreateHome(home);

        result.Should().BeEquivalentTo(home);
    }

    [TestMethod]
    public void GetHomeMembersTest()
    {
        var homeId = Guid.NewGuid();
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Snow",
                Email = "mail@mail.com",
                Password = "password@123"
            }
        };

        _homeRepositoryMock?.Setup(x => x.GetHomeMembers(homeId)).Returns(users);

        var result = _homeLogic?.GetHomeMembers(homeId);

        result.Should().BeEquivalentTo(users);
    }

    [TestMethod]
    public void GetHomeMembers_ShouldThrowEmptyException_WhenNoMembersFound()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomeMembers(homeId)).Returns([]);

        Action act = () => _homeLogic?.GetHomeMembers(homeId);

        act.Should().Throw<EmptyException>()
            .WithMessage("No members found for this home.");
    }


    [TestMethod]
    public void AddMemberTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@asdas.com",
            Password = "password@123"
        };

        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };

        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [user],
            MemberCount = 20,
            Devices = homeDevices
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _homeRepositoryMock?.Setup(x => x.GetHomeMembers(homeId)).Returns(home.Members);

        _homeRepositoryMock?.Setup(x => x.AddMember(homeId, userId)).Returns(home);

        var result = _homeLogic?.AddMember(homeId, userId);

        result.Should().BeEquivalentTo(home);
    }


    [TestMethod]
    public void GetHomeTest()
    {
        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };

        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var homeId = Guid.NewGuid();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = user.Id,
            Members = [user],
            MemberCount = 5,
            Devices = homeDevices
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        var result = _homeLogic?.GetHome(homeId);

        result.Should().BeEquivalentTo(home);
    }

    [TestMethod]
    public void GetHome_ShouldThrowNotValidDataException_WhenHomeNotFound()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns((Home?)null);

        Action act = () => _homeLogic?.GetHome(homeId);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Home not found.");
    }


    [TestMethod]
    public void GetHomesByUserTest()
    {
        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };

        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };

        var homes = new List<Home>
        {
            new Home
            {
                Id = Guid.NewGuid(),
                Location = "Home",
                Latitude = "123",
                Longitude = "123",
                HomeOwner = user.Id,
                Members = [user],
                MemberCount = 5,
                Devices = homeDevices
            }
        };

        _homeRepositoryMock?.Setup(x => x.GetHomesByUser(user.Id)).Returns(homes);

        var result = _homeLogic?.GetHomesByUser(user.Id);

        result.Should().BeEquivalentTo(homes);
    }

    [TestMethod]
    public void GetHomesByUser_ShouldThrowEmptyException_WhenNoHomesFound()
    {
        var userId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomesByUser(userId)).Returns([]);

        Action act = () => _homeLogic?.GetHomesByUser(userId);

        act.Should().Throw<EmptyException>()
            .WithMessage("No homes found for this user.");
    }


    [TestMethod]
    public void UpdatePermissions_ShouldAddPermissions_WhenPermissionsAreTrue()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permissionRequest = new PermissionRequest { Value = "CanAddMembers", Enable = true };

        // Act
        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);

        var permissionRequest2 = new PermissionRequest { Value = "CanAsociateDevices", Enable = true };

        // Act
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest2);

        // Assert
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAsociateDevices"), Times.Once);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldRemovePermissions_WhenPermissionsAreFalseAndExist()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permissionRequest = new PermissionRequest { Value = "CanAddMembers", Enable = false };

        var permissionRequest2 = new PermissionRequest { Value = "CanAsociateDevices", Enable = false };


        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAddMembers")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAsociateDevices")).Returns(true);

        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest2);

        // Assert
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAsociateDevices"), Times.Once);

    }

    [TestMethod]
    public void UpdatePermissions_ShouldAddAllPermissions_WhenAllPermissionsAreTrue()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permissionRequest = new PermissionRequest { Value = "CanAddMembers", Enable = true };

        var permissionRequest2 = new PermissionRequest { Value = "CanAsociateDevices", Enable = true };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest2);

        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAsociateDevices"), Times.Once);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldRemoveAllPermissions_WhenAllPermissionsAreFalse()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permissionRequest = new PermissionRequest { Value = "CanAddMembers", Enable = false };

        var permissionRequest2 = new PermissionRequest { Value = "CanAsociateDevices", Enable = false };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest2);

        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAddMembers")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAsociateDevices")).Returns(true);

        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAsociateDevices"), Times.Once);
    }

    [TestMethod]
    public void AddMember_ShouldThrowConflictException_WhenMemberLimitIsReached()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };

        var members = new List<User> { user };

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = members,
            MemberCount = 1
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeMembers(homeId)).Returns(members);

        Action act = () => _homeLogic?.AddMember(homeId, Guid.NewGuid());

        act.Should().Throw<ConflictException>()
            .WithMessage("House is full. Member limit has been reached.");
    }

    [TestMethod]
    public void AddDevice_ShouldAddDevice_WhenCalled()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), DeviceId = deviceId };

        _homeRepositoryMock?.Setup(x => x.AddDevice(homeId, deviceId)).Returns(homeDevice);

        // Act
        var result = _homeLogic?.AddDevice(homeId, deviceId);

        // Assert
        result.Should().BeEquivalentTo(homeDevice);
        _homeRepositoryMock?.Verify(x => x.AddDevice(homeId, deviceId), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnListOfDevices_WhenHomeIdIsValid()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var devices = new List<HomeDevice>
            {
                new HomeDevice { Id = Guid.NewGuid(), DeviceId = Guid.NewGuid() },
                new HomeDevice { Id = Guid.NewGuid(), DeviceId = Guid.NewGuid() }
            };

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId)).Returns(devices);

        // Act
        var result = _homeLogic?.GetHomeDevices(homeId);

        // Assert
        result.Should().BeEquivalentTo(devices);
        _homeRepositoryMock?.Verify(x => x.GetHomeDevices(homeId), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnException_WhenNoDevicesFound()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId)).Returns([]);

        Action act = () => _homeLogic?.GetHomeDevices(homeId);

        act.Should().Throw<EmptyException>()
            .WithMessage("No devices found for this home.");
    }


    [TestMethod]
    public void GetHomes_ShouldReturnException_WhenNoHomesFound()
    {
        _homeRepositoryMock?.Setup(x => x.GetHomes()).Returns([]);

        Action act = () => _homeLogic?.GetHomes();

        act.Should().Throw<EmptyException>()
            .WithMessage("No homes found.");
    }


    [TestMethod]
    public void CreateNotificationSensor_ShouldCreateNotification_WhenCalled()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var device = new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Sensor, Description = "description", Photo = "photo" };

        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device };

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        var sensorRequest = new SensorRequest { Event = "open" };
        var notifications = new List<Notification> { new Notification { Id = Guid.NewGuid(), HardwareId = hardwareId } };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId)).Returns(home.Devices);
        _notificationRepositoryMock?.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, sensorRequest)).Returns(notifications);

        // Act
        var result = _homeLogic?.CreateNotificationSensor(homeId, hardwareId, sensorRequest);

        // Assert
        result.Should().BeEquivalentTo(notifications);
        _notificationRepositoryMock?.Verify(x => x.CreateNotificationSensor(homeId, hardwareId, sensorRequest), Times.Once);
    }

    [TestMethod]
    public void CreateNotificationCamera_ShouldCreateNotification_WhenCalled()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var device = new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" };

        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device };

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        var sensorRequest = new SensorRequest { Event = "person-detected" };
        var notifications = new List<Notification> { new Notification { Id = Guid.NewGuid(), HardwareId = hardwareId } };


        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId)).Returns(home.Devices);
        _notificationRepositoryMock?.Setup(x => x.CreateNotificationCamera(homeId, hardwareId, sensorRequest)).Returns(notifications);

        // Act
        var result = _homeLogic?.CreateNotificationCamera(homeId, hardwareId, sensorRequest);

        // Assert
        result.Should().BeEquivalentTo(notifications);
        _notificationRepositoryMock?.Verify(x => x.CreateNotificationCamera(homeId, hardwareId, sensorRequest), Times.Once);
    }

    [TestMethod]
    public void CreateNotificationSensor_ShouldThrowExceptionWhenEventNotValid()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        var sensorRequest = new SensorRequest { Event = "notAValidEvent" };
        var notifications = new List<Notification>
        {
            new Notification { Id = Guid.NewGuid(), HardwareId = hardwareId }
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _notificationRepositoryMock?.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, sensorRequest)).Returns(notifications);

        Action act = () => _homeLogic?.CreateNotificationSensor(homeId, hardwareId, sensorRequest);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Event must be open or close");
    }

    [TestMethod]
    public void ChangeHomeDeviceName_ShouldChangeName_WhenCalled()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var name = "newName";

        var device = new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" };

        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device, Name = "oldName" };
        var homeDeviceExpected = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device, Name = "newName" };

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId)).Returns(home.Devices);
        _homeRepositoryMock?.Setup(x => x.ChangeHomeDeviceName(homeId, hardwareId, name)).Returns(homeDeviceExpected);

        var result = _homeLogic?.ChangeHomeDeviceName(homeId, hardwareId, name);

        result?.Name.Should().Be(name);
        _homeRepositoryMock?.Verify(x => x.ChangeHomeDeviceName(homeId, hardwareId, name), Times.Once);
    }

    [TestMethod]
    public void ChangeHomeDeviceName_ShouldThrowNotValidDataException_WhenHomeNotFound()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var name = "newName";

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns((Home?)null);

        Action act = () => _homeLogic?.ChangeHomeDeviceName(homeId, hardwareId, name);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Home not found");
    }

    [TestMethod]
    public void ChangeHomeDeviceName_ShouldThrowNotValidDataException_WhenDeviceNotFound()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var name = "newName";

        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId)).Returns([]);

        Action act = () => _homeLogic?.ChangeHomeDeviceName(homeId, hardwareId, name);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Device not found");
    }
}
