using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeControllerTest
{
    private Company? _company;

    [TestInitialize]
    public void Setup()
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
    }

    [TestMethod]
    public void CreateHome_WhenAllPropertiesOk()
    {
        var httpContext = new DefaultHttpContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        httpContext.Items[0] = user;

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "location",
            MemberCount = 5,
            HomeOwner = user.Id
        };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            HomeOwner = home.HomeOwner
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.CreateHome(It.IsAny<Home>())).Returns(homeRequest.ToArgs());

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            }
        };

        IActionResult act = controller.CreateHome(homeRequest);

        var homeRequestObject = homeRequest.ToArgs();
        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            HomeOwner = homeRequestObject.HomeOwner
        };

        var expected = new OkObjectResult(homeResponse);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomes_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "location",
            MemberCount = 5,
            HomeOwner = user.Id
        };

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            HomeOwner = home.HomeOwner
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.GetHomes()).Returns(homes);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHomes();

        var homeRequestObject = homeRequest.ToArgs();
        homeRequestObject.HomeOwner = user.Id;

        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            HomeOwner = homeRequestObject.HomeOwner
        };

        var expected = new OkObjectResult(new List<HomeResponse> { homeResponse });

        act.Should().BeEquivalentTo(expected);
    }


    [TestMethod]
    public void GetHome_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "location",
            MemberCount = 5,
            HomeOwner = user.Id
        };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            HomeOwner = home.HomeOwner
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.GetHome(It.IsAny<Guid>())).Returns(home);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHome(home.Id);

        var homeRequestObject = homeRequest.ToArgs();
        homeRequestObject.HomeOwner = user.Id;

        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            HomeOwner = homeRequestObject.HomeOwner
        };

        var expected = new OkObjectResult(homeResponse);

        act.Should().BeEquivalentTo(expected);
    }


    [TestMethod]
    public void GetHomeMembers_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var users = new List<User> { user };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "location",
            MemberCount = 5,
            Devices = [],
            HomeOwner = user.Id,
            Members = users
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.GetHomeMembers(It.IsAny<Guid>())).Returns(users);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHomeMembers(home.Id);

        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        var expectedResponse = new List<GetHomeMembersResponse>
        {
            new()
            {
                Name = user.Name,
                Email = user.Email
            }
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());

        homeLogic.Verify(x => x.GetHomeMembers(home.Id), Times.Once);
    }

    [TestMethod]
    public void AddMemberToHome_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var addMemberRequest = new AddMemberRequest
        {
            UserId = userId.ToString()
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            MemberCount = 1,
            Devices = [],
            HomeOwner = Guid.NewGuid(),
            Members = []
        };

        var memberSetting = new MemberSetting
        {
            HomeId = homeId,
            UserId = userId,
            Permissions = []
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.AddMember(homeId, userId)).Returns(home);
        memberSettingLogic.Setup(x => x.CreateMemberSetting(homeId, userId)).Returns(memberSetting);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.AddMemberToHome(homeId, addMemberRequest);

        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        var expectedResponse = new AddMemberResponse
        {
            Home = home,
            MemberSetting = memberSetting
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());

        homeLogic.Verify(x => x.AddMember(homeId, userId), Times.Once);
        memberSettingLogic.Verify(x => x.CreateMemberSetting(homeId, userId), Times.Once);
    }

    [TestMethod]
    public void AddDeviceToHome_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var device = new Device
        {

            Id = deviceId,
            Company = _company,
            Name = "Camera",
            Model = "XYZ",
            DeviceType = DeviceType.Camera,
            Description = "Outdoor camera",
            Photo = "photo1.jpg"
        };

        var homeDevice = new HomeDevice { HardwareId = Guid.NewGuid(), DeviceId = deviceId, Device = device, state = false };

        var homeDevices = new List<HomeDevice> { homeDevice };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            MemberCount = 5,
            Devices = homeDevices,
            HomeOwner = Guid.NewGuid()
        };

        var homeDeviceRequest = new HomeDeviceRequest { DeviceId = deviceId };

        var homeDeviceResponse = new HomeDeviceResponse
        {
            HardwareId = homeDevice.HardwareId,
            Device = device,
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.AddDevice(homeId, deviceId)).Returns(homeDevice);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.AddDeviceToHome(homeId, homeDeviceRequest);

        var expected = new OkObjectResult(homeDeviceResponse);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomeDevices_WhenHomeHasDevices()
    {
        var homeId = Guid.NewGuid();
        var device1 = new Device
        {
            Company = _company,
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "XYZ",
            DeviceType = DeviceType.Camera,
            Description = "Outdoor camera",
            Photo = "photo1.jpg"
        };
        var device2 = new Device
        {
            Company = _company,
            Id = Guid.NewGuid(),
            Name = "Thermostat",
            Model = "ABC",
            DeviceType = DeviceType.Sensor,
            Description = "Smart thermostat",
            Photo = "photo2.jpg"
        };
        var devices = new List<HomeDevice>
        {
            new HomeDevice { DeviceId = device1.Id, Device = device1 },
            new HomeDevice { DeviceId = device2.Id, Device = device2 }
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            MemberCount = 1,
            Devices = devices,
            HomeOwner = Guid.NewGuid()
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeDevices(homeId)).Returns(home.Devices);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHomeDevices(homeId);

        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        okResult.Value.Should().BeEquivalentTo(devices, options => options.WithStrictOrdering());

        homeLogic.Verify(x => x.GetHomeDevices(homeId), Times.Once);
    }

    [TestMethod]
    public void CreateNotificationOpenSensor_WhenAllPropertiesOk()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorRequest = new SensorRequest
        {
            Event = "open"
        };

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "Sensor opened",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            HardwareId = hardwareId,
            UserId = Guid.NewGuid()

        };

        var notifications = new List<Notification> { notification };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, It.IsAny<SensorRequest>()))
            .Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationOpenSensor(homeId, hardwareId);

        var expected = new OkObjectResult(notifications);
        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void CreateNotificationCamera_WhenAllPropertiesOk()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorRequest = new SensorRequest
        {
            Event = "person-detected"
        };

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "person-detected",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            HardwareId = hardwareId,
            UserId = Guid.NewGuid()

        };

        var notifications = new List<Notification> { notification };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateNotificationCamera(homeId, hardwareId, It.IsAny<SensorRequest>()))
            .Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationPersonDetectedCamera(homeId, hardwareId);

        var expected = new OkObjectResult(notifications);
        act.Should().BeEquivalentTo(expected);
    }
}
