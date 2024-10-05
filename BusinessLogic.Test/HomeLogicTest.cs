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
        _homeLogic = new HomeLogic(_homeRepositoryMock.Object, _memberSettingRepositoryMock.Object);
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

        var homes = new List<Home>
        {
            new Home
            {
                Id = Guid.NewGuid(),
                Location = "Home",
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
            HomeOwner = Guid.NewGuid(),
            Members = [user],
            MemberCount = 5,
            Devices = homeDevices
        };

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
    public void AddMemberTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.asdas.com",
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
            HomeOwner = Guid.NewGuid(),
            Members = [user],
            MemberCount = 5,
            Devices = homeDevices
        };

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
    public void UpdatePermissions_ShouldAddPermissions_WhenPermissionsAreTrue()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permissionRequest = new PermissionRequest
        {
            CanAddMembers = true,
            CanAsociateDevices = true,
            CanGetNotifications = false,
            CanListDevices = true
        };

        // Act
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);

        // Assert
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAsociateDevices"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanListDevices"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanGetNotifications"), Times.Never);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldRemovePermissions_WhenPermissionsAreFalseAndExist()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permissionRequest = new PermissionRequest
        {
            CanAddMembers = false,
            CanAsociateDevices = false,
            CanGetNotifications = false,
            CanListDevices = false
        };

        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAddMembers")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAsociateDevices")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanGetNotifications")).Returns(false);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanListDevices")).Returns(true);

        // Act
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);

        // Assert
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAsociateDevices"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanListDevices"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanGetNotifications"), Times.Never);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldAddAllPermissions_WhenAllPermissionsAreTrue()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permissionRequest = new PermissionRequest
        {
            CanAddMembers = true,
            CanAsociateDevices = true,
            CanGetNotifications = true,
            CanListDevices = true
        };

        // Act
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);

        // Assert
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanAsociateDevices"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanGetNotifications"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.AddPermission(homeId, userId, "CanListDevices"), Times.Once);
    }

    [TestMethod]
    public void UpdatePermissions_ShouldRemoveAllPermissions_WhenAllPermissionsAreFalse()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permissionRequest = new PermissionRequest
        {
            CanAddMembers = false,
            CanAsociateDevices = false,
            CanGetNotifications = false,
            CanListDevices = false
        };

        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAddMembers")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanAsociateDevices")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanGetNotifications")).Returns(true);
        _memberSettingRepositoryMock?.Setup(r => r.HasPermission(homeId, userId, "CanListDevices")).Returns(true);

        // Act
        _homeLogic?.UpdatePermissions(homeId, userId, permissionRequest);

        // Assert
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAddMembers"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanAsociateDevices"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanGetNotifications"), Times.Once);
        _memberSettingRepositoryMock?.Verify(r => r.RemovePermission(homeId, userId, "CanListDevices"), Times.Once);
    }
}
