using System.Diagnostics.CodeAnalysis;
using BusinessLogic.DataAccessInterfaces;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
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
            Name = "Home",
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
            Name = "Home",
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
            Name = "Home",
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
            Name = "Home",
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
                Name = "Home",
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
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permission = "CanAddMembers";
        var permission2 = "CanAsociateDevices";
        var addPermission = true;

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeLogic?.UpdatePermissions(homeId, userId, permission, addPermission);

        _homeLogic?.UpdatePermissions(homeId, userId, permission2, addPermission);

        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAsociateDevices"), Times.Once);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldRemovePermissions_WhenPermissionsAreFalseAndExist()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permission = "CanAddMembers";
        var permission2 = "CanAsociateDevices";
        var addPermission = false;


        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAddMembers")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAsociateDevices")).Returns(true);

        _homeLogic?.UpdatePermissions(homeId, userId, permission, addPermission);
        _homeLogic?.UpdatePermissions(homeId, userId, permission2, addPermission);

        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAsociateDevices"), Times.Once);

    }

    [TestMethod]
    public void UpdatePermissions_ShouldAddAllPermissions_WhenAllPermissionsAreTrue()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permission = "CanAddMembers";
        var permission2 = "CanAsociateDevices";
        var addPermission = true;

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _homeLogic?.UpdatePermissions(homeId, userId, permission, addPermission);
        _homeLogic?.UpdatePermissions(homeId, userId, permission2, addPermission);

        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAsociateDevices"), Times.Once);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldRemoveAllPermissions_WhenAllPermissionsAreFalse()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = userId,
            Members = [],
            MemberCount = 5
        };

        var permission = "CanAddMembers";
        var permission2 = "CanAsociateDevices";
        var addPermission = false;

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        _homeLogic?.UpdatePermissions(homeId, userId, permission, addPermission);
        _homeLogic?.UpdatePermissions(homeId, userId, permission2, addPermission);

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
            Name = "Home",
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
        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), DeviceId = deviceId };

        _homeRepositoryMock?.Setup(x => x.AddDevice(homeId, deviceId)).Returns(homeDevice);

        var result = _homeLogic?.AddDevice(homeId, deviceId);

        result.Should().BeEquivalentTo(homeDevice);
        _homeRepositoryMock?.Verify(x => x.AddDevice(homeId, deviceId), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnListOfDevices_WhenHomeIdIsValid()
    {
        var homeId = Guid.NewGuid();
        var devices = new List<HomeDevice>
            {
                new HomeDevice { Id = Guid.NewGuid(), DeviceId = Guid.NewGuid() },
                new HomeDevice { Id = Guid.NewGuid(), DeviceId = Guid.NewGuid() }
            };

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(devices);

        var result = _homeLogic?.GetHomeDevices(homeId, null);

        result.Should().BeEquivalentTo(devices);
        _homeRepositoryMock?.Verify(x => x.GetHomeDevices(homeId, null), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnException_WhenNoDevicesFound()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns([]);

        Action act = () => _homeLogic?.GetHomeDevices(homeId, null);

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
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var device = new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.WindowSensor, Description = "description", Photo = "photo" };

        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device };

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        var sensorEvent = "open";
        var notifications = new List<Notification> { new Notification { Id = Guid.NewGuid(), HardwareId = hardwareId } };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);
        _notificationRepositoryMock?.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, sensorEvent)).Returns(notifications);

        var result = _homeLogic?.CreateNotificationSensor(homeId, hardwareId, sensorEvent);

        result.Should().BeEquivalentTo(notifications);
        _notificationRepositoryMock?.Verify(x => x.CreateNotificationSensor(homeId, hardwareId, sensorEvent), Times.Once);
    }

    [TestMethod]
    public void CreateNotificationCamera_ShouldCreateNotification_WhenCalled()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var device = new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" };

        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device };

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        var sensorEvent = "person-detected";
        var notifications = new List<Notification> { new Notification { Id = Guid.NewGuid(), HardwareId = hardwareId } };


        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);
        _notificationRepositoryMock?.Setup(x => x.CreateNotificationCamera(homeId, hardwareId, sensorEvent)).Returns(notifications);

        var result = _homeLogic?.CreateNotificationCamera(homeId, hardwareId, sensorEvent);

        result.Should().BeEquivalentTo(notifications);
        _notificationRepositoryMock?.Verify(x => x.CreateNotificationCamera(homeId, hardwareId, sensorEvent), Times.Once);
    }

    [TestMethod]
    public void CreateNotificationSensor_ShouldThrowExceptionWhenEventNotValid()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        var sensorEvent = "notValidEvent";
        var notifications = new List<Notification>
        {
            new Notification { Id = Guid.NewGuid(), HardwareId = hardwareId }
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _notificationRepositoryMock?.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, sensorEvent)).Returns(notifications);

        Action act = () => _homeLogic?.CreateNotificationSensor(homeId, hardwareId, sensorEvent);

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
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);
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
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns([]);

        Action act = () => _homeLogic?.ChangeHomeDeviceName(homeId, hardwareId, name);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Device not found");
    }

    [TestMethod]
    public void AddRoom_ShouldAddRoom_WhenCalled()
    {
        var homeId = Guid.NewGuid();
        var name = "room";

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };
        var room = new Room { Id = Guid.NewGuid(), Name = name };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.AddRoom(homeId, name)).Returns(room);

        var result = _homeLogic?.AddRoom(homeId, name);

        result.Should().BeEquivalentTo(room);
        _homeRepositoryMock?.Verify(x => x.AddRoom(homeId, name), Times.Once);
    }

    [TestMethod]
    public void GetRooms_ShouldReturnListOfRooms_WhenHomeIdIsValid()
    {
        var homeId = Guid.NewGuid();
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), Name = "room1" },
            new Room { Id = Guid.NewGuid(), Name = "room2" }
        };

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetRooms(homeId)).Returns(rooms);

        var result = _homeLogic?.GetRooms(homeId);

        result.Should().BeEquivalentTo(rooms);
        _homeRepositoryMock?.Verify(x => x.GetRooms(homeId), Times.Once);
    }

    [TestMethod]
    public void AddDeviceToRoom_ShouldAddDeviceToRoom_WhenCalled()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var device = new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" };

        var homeDevice = new HomeDevice { Id = Guid.NewGuid(), HardwareId = hardwareId, Device = device };

        var homeDevices = new List<HomeDevice> { homeDevice };

        var room = new Room { Id = roomId, Name = "room" };
        var roomWithDevice = new Room { Id = roomId, Name = "room", Devices = homeDevices };

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            Devices = [homeDevice],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetRooms(homeId)).Returns([room]);
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);
        _homeRepositoryMock?.Setup(x => x.AddDeviceToRoom(homeId, hardwareId, roomId)).Returns(roomWithDevice);

        var expectedResponse = new Room() { Id = roomId, Name = "room", Devices = homeDevices };

        var result = _homeLogic?.AddDeviceToRoom(homeId, hardwareId, roomId);

        result.Should().BeEquivalentTo(expectedResponse);

        _homeRepositoryMock?.Verify(x => x.AddDeviceToRoom(homeId, hardwareId, roomId), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevicesByRoom_ShouldReturnListOfDevices_WhenHomeIdAndRoomIdAreValid()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var devices = new List<HomeDevice>
        {
            new HomeDevice { Id = Guid.NewGuid(), DeviceId = Guid.NewGuid() },
            new HomeDevice { Id = Guid.NewGuid(), DeviceId = Guid.NewGuid() }
        };

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, roomId)).Returns(devices);

        var result = _homeLogic?.GetHomeDevices(homeId, roomId);

        result.Should().BeEquivalentTo(devices);
        _homeRepositoryMock?.Verify(x => x.GetHomeDevices(homeId, roomId), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevicesByRoom_ShouldReturnException_WhenNoDevicesFound()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, roomId)).Returns([]);

        Action act = () => _homeLogic?.GetHomeDevices(homeId, roomId);

        act.Should().Throw<EmptyException>()
            .WithMessage("No devices found for this room in this home.");
    }

    [TestMethod]
    public void GetHomeDevicesByRoom_ShouldReturnException_WhenHomeIdIsInvalid()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns((List<HomeDevice>?)null);

        Action act = () => _homeLogic?.GetHomeDevices(homeId, null);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Home not found.");
    }

    [TestMethod]
    public void GetHomeDevicesByRoom_WhenNoDevicesForHome()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns([]);

        Action act = () => _homeLogic?.GetHomeDevices(homeId, null);

        act.Should().Throw<EmptyException>()
            .WithMessage("No devices found for this home.");

    }

    [TestMethod]
    public void ChangeHomeName_ShouldChangeName_WhenCalled()
    {
        var homeId = Guid.NewGuid();
        var oldName = "Home";
        var newName = "New Home";

        var home = new Home
        {
            Id = homeId,
            Name = oldName,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        var homeExpected = new Home
        {
            Id = homeId,
            Name = newName,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.ChangeHomeName(homeId, oldName)).Returns(homeExpected);

        var result = _homeLogic?.ChangeHomeName(homeId, oldName);

        result.Should().BeEquivalentTo(homeExpected);
        _homeRepositoryMock?.Verify(x => x.ChangeHomeName(homeId, oldName), Times.Once);
    }

    [TestMethod]
    public void ChangeHomeName_ShouldThrowNotValidDataException_WhenHomeNotFound()
    {
        var homeId = Guid.NewGuid();
        var oldName = "Home";

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns((Home?)null);

        Action act = () => _homeLogic?.ChangeHomeName(homeId, oldName);

        act.Should().Throw<NotValidDataException>()
            .WithMessage("Home not found");
    }
    
    [TestMethod]
    public void AddDeviceToRoom_ShouldThrowException_WhenHomeDoesNotExist()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns((Home)null);

        Action act = () => _homeLogic?.AddDeviceToRoom(homeId, hardwareId, roomId);

        act.Should().Throw<NotValidDataException>().WithMessage("Home not found");

        _homeRepositoryMock?.Verify(x => x.GetHome(homeId), Times.Once);
    }
    
    [TestMethod]
    public void AddDeviceToRoom_ShouldThrowException_WhenRoomDoesNotExist()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetRooms(homeId)).Returns(new List<Room>());

        Action act = () => _homeLogic?.AddDeviceToRoom(homeId, hardwareId, roomId);

        act.Should().Throw<NotValidDataException>().WithMessage("Room not found");

        _homeRepositoryMock?.Verify(x => x.GetRooms(homeId), Times.Once);
    }
    
    [TestMethod]
    public void AddDeviceToRoom_ShouldThrowException_WhenDeviceDoesNotExist()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var room = new Room { Id = roomId, Name = "room" };

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5,
            Devices = []
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetRooms(homeId)).Returns(new List<Room> { room });
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);

        Action act = () => _homeLogic?.AddDeviceToRoom(homeId, hardwareId, roomId);

        act.Should().Throw<NotValidDataException>().WithMessage("Device not found");

        _homeRepositoryMock?.Verify(x => x.GetHomeDevices(homeId, null), Times.Once);
    }

    [TestMethod]
    public void AddDeviceToRoom_ShouldThrowException_WhenDeviceCouldNotBeAdded()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var device = new Device
        {
            Id = Guid.NewGuid(),
            Company = _company,
            Name = "device",
            Model = "model",
            DeviceType = DeviceType.Camera,
            Description = "description",
            Photo = "photo"
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            HardwareId = hardwareId,
            Device = device
        };

        var room = new Room { Id = roomId, Name = "room" };

        var home = new Home
        {
            Id = homeId,
            Name = "Home",
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            HomeOwner = Guid.NewGuid(),
            Members = [],
            MemberCount = 5,
            Devices = [homeDevice]
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);
        _homeRepositoryMock?.Setup(x => x.GetRooms(homeId)).Returns(new List<Room> { room });
        _homeRepositoryMock?.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);
        _homeRepositoryMock?.Setup(x => x.AddDeviceToRoom(homeId, hardwareId, roomId)).Returns((Room)null);

        Action act = () => _homeLogic?.AddDeviceToRoom(homeId, hardwareId, roomId);

        act.Should().Throw<NotValidDataException>().WithMessage("Device could not be added to room");

        _homeRepositoryMock?.Verify(x => x.AddDeviceToRoom(homeId, hardwareId, roomId), Times.Once);
    }


}
