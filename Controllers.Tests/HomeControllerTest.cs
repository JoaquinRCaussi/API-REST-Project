using System.Diagnostics.CodeAnalysis;
using System.Net;
using BusinessLogic;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using WebApi.Controllers;
using WebApi.Filters;
using WebApi.Models.In;
using WebApi.Models.Out;

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
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            HomeOwner = user.Id
        };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            HomeOwner = home.HomeOwner,
            Latitude = home.Latitude,
            Longitude = home.Longitude
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
            Latitude = homeRequestObject.Latitude,
            Longitude = homeRequestObject.Longitude,
            MemberCount = homeRequestObject.MemberCount,
            HomeOwner = homeRequestObject.HomeOwner
        };

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateHome),
            nameof(HomeController).Replace("Controller", ""),
            new { id = homeRequestObject.Id },
            new HomeResponse
            {
                Location = homeRequestObject.Location,
                Latitude = homeRequestObject.Latitude,
                Longitude = homeRequestObject.Longitude,
                MemberCount = homeRequestObject.MemberCount,
                HomeOwner = homeRequestObject.HomeOwner
            }
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
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
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            HomeOwner = user.Id
        };

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            Latitude = home.Latitude,
            Longitude = home.Longitude,
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
            Latitude = homeRequestObject.Latitude,
            Longitude = homeRequestObject.Longitude,
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
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            HomeOwner = user.Id
        };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            Latitude = home.Latitude,
            Longitude = home.Longitude,
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
            Latitude = homeRequestObject.Latitude,
            Longitude = homeRequestObject.Longitude,
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
            Latitude = "123",
            Longitude = "123",
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
            Latitude = "123",
            Longitude = "123",
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

        var homeDevice = new HomeDevice { HardwareId = Guid.NewGuid(), DeviceId = deviceId, Device = device, State = false };

        var homeDevices = new List<HomeDevice> { homeDevice };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            Latitude = "123",
            Longitude = "123",
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

        var expected = new CreatedAtActionResult(
            nameof(controller.AddDeviceToHome),
            nameof(HomeController).Replace("Controller", ""),
            new { id = home.Id },
            new HomeDeviceResponse
            {
                HardwareId = homeDevice.HardwareId,
                Device = device
            }
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
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
            DeviceType = DeviceType.WindowSensor,
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
            Latitude = "123",
            Longitude = "123",
            MemberCount = 1,
            Devices = devices,
            HomeOwner = Guid.NewGuid()
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeDevices(homeId, null)).Returns(home.Devices);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHomeDevices(homeId);

        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        okResult.Value.Should().BeEquivalentTo(devices, options => options.WithStrictOrdering());

        homeLogic.Verify(x => x.GetHomeDevices(homeId, null), Times.Once);
    }

    [TestMethod]
    public void CreateNotificationOpenSensor_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorEvent = "open";

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
        homeLogic.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, sensorEvent)).Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationOpenSensor(homeId, hardwareId);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateNotificationOpenSensor),
            nameof(HomeController).Replace("Controller", ""),
            new { id = notification.Id },
            notifications
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void CreateNotificationCloseSensor_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorEvent = "close";

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "Sensor closed",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            HardwareId = hardwareId,
            UserId = Guid.NewGuid()
        };

        var notifications = new List<Notification> { notification };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, sensorEvent))
            .Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationCloseSensor(homeId, hardwareId);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateNotificationCloseSensor),
            nameof(HomeController).Replace("Controller", ""),
            new { id = notification.Id },
            notifications
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void CreateNotificationCameraPersonDetected_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorEvent = "person-detected";

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
        homeLogic.Setup(x => x.CreateNotificationCamera(homeId, hardwareId, sensorEvent))
            .Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationPersonDetectedCamera(homeId, hardwareId);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateNotificationPersonDetectedCamera),
            nameof(HomeController).Replace("Controller", ""),
            new { id = notification.Id },
            notifications
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void CreateNotificationCameraMovementDetected_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorEvent = "movement-detected";

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "movement-detected",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            HardwareId = hardwareId,
            UserId = Guid.NewGuid()

        };

        var notifications = new List<Notification> { notification };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateNotificationCamera(homeId, hardwareId, sensorEvent))
            .Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationMovementDetectedCamera(homeId, hardwareId);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateNotificationMovementDetectedCamera),
            nameof(HomeController).Replace("Controller", ""),
            new { id = notification.Id },
            notifications
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void CreateNotificationSensorMovementDetected_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var sensorRequest = new SensorRequest
        {
            Event = "movement-detected"
        };

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "movement-detected",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            HardwareId = hardwareId,
            UserId = Guid.NewGuid()

        };

        var notifications = new List<Notification> { notification };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateNotificationSensor(homeId, hardwareId, "movement-detected"))
            .Returns(notifications);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.CreateNotificationMovementDetectedSensor(homeId, hardwareId);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateNotificationMovementDetectedSensor),
            nameof(HomeController).Replace("Controller", ""),
            new { id = notification.Id },
            notifications
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void GetHomes_ShouldReturnNoContent()
    {
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomes())
            .Throws(new EmptyException("No homes found"));

        var controller = new HomeController(homeLogic.Object, null);

        Action act = () => controller.GetHomes();

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No homes found")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        homeLogic.VerifyAll();
    }

    [TestMethod]
    public void GetHomeMembers_ShouldReturnNoContent()
    {
        var homeId = Guid.NewGuid();
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeMembers(homeId))
            .Throws(new EmptyException("No members found for this home."));

        var controller = new HomeController(homeLogic.Object, null);

        Action act = () => controller.GetHomeMembers(homeId);

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No members found for this home.")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        homeLogic.VerifyAll();
    }

    [TestMethod]
    public void GetHomeById_ShouldReturnNoContent()
    {
        var homeId = Guid.NewGuid();
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHome(homeId))
            .Throws(new NotValidDataException("Home not found."));

        var controller = new HomeController(homeLogic.Object, null);

        Action act = () => controller.GetHome(homeId);

        act.Should().Throw<NotValidDataException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new NotValidDataException("Home not found.")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        homeLogic.VerifyAll();
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnNoContent()
    {
        var homeId = Guid.NewGuid();
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeDevices(homeId, null))
            .Throws(new EmptyException("No devices found for this home."));

        var controller = new HomeController(homeLogic.Object, null);

        Action act = () => controller.GetHomeDevices(homeId);

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No devices found for this home.")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        homeLogic.VerifyAll();
    }

    [TestMethod]
    public void ChangeHomeDeviceName_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var name = "NewName";

        var device = new Device
        {
            Company = _company,
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "XYZ",
            DeviceType = DeviceType.Camera,
            Description = "Outdoor camera",
            Photo = "photo1.jpg"
        };

        var homeDevice = new HomeDevice { HardwareId = hardwareId, DeviceId = device.Id, Device = device, State = false };

        var homeDevices = new List<HomeDevice> { homeDevice };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices,
            HomeOwner = Guid.NewGuid()
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.ChangeHomeDeviceName(homeId, hardwareId, name)).Returns(homeDevice);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.ChangeHomeDeviceName(homeId, hardwareId, name);

        var expected = new OkObjectResult(homeDevice);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddRoomToHome_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var roomName = "Living Room";

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = roomName
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = [],
            HomeOwner = Guid.NewGuid(),
            Rooms = []
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.AddRoom(homeId, roomName)).Returns(room);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.AddRoomToHome(homeId, roomName);

        var expected = new CreatedAtActionResult(
            nameof(controller.AddRoomToHome),
            nameof(HomeController).Replace("Controller", ""),
            new { id = room.Id },
            room
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void GetRooms_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var roomName = "Living Room";

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = roomName
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = [],
            HomeOwner = Guid.NewGuid(),
            Rooms = [room]
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetRooms(homeId)).Returns(home.Rooms);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetRooms(homeId);

        var getRoomsResponse = new GetRoomsResponse(home.Rooms);
        var expected = new OkObjectResult(getRoomsResponse.ToArgs());

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddDeviceToRoom_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var device = new Device
        {
            Company = _company,
            Id = Guid.NewGuid(),
            Name = "Camera",
            Model = "XYZ",
            DeviceType = DeviceType.Camera,
            Description = "Outdoor camera",
            Photo = "photo1.jpg"
        };

        var homeDevice = new HomeDevice { HardwareId = hardwareId, DeviceId = device.Id, Device = device, State = false, Name = device.Name };

        var homeDevices = new List<HomeDevice> { homeDevice };

        var room = new Room
        {
            Id = roomId,
            Name = "Living Room",
            Devices = homeDevices
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices,
            HomeOwner = Guid.NewGuid()
        };

        var addDeviceToRoomReq = new AddDeviceToRoomRequest() { HardwareId = hardwareId };

        var addDeviceToRoomRes = new DeviceRoomResponse()
        {
            HardwareId = homeDevice.HardwareId,
            DeviceName = homeDevice.Device.Name,
            RoomName = room.Name
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.AddDeviceToRoom(homeId, hardwareId, roomId)).Returns(room);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.AddDeviceToRoom(homeId, roomId, addDeviceToRoomReq);

        var expected = new OkObjectResult(addDeviceToRoomRes);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomeDevicesByRoomId_WhenAllPropertiesOk()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

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
            DeviceType = DeviceType.WindowSensor,
            Description = "Smart thermostat",
            Photo = "photo2.jpg"
        };
        var devices = new List<HomeDevice>
        {
            new HomeDevice { DeviceId = device1.Id, Device = device1 },
            new HomeDevice { DeviceId = device2.Id, Device = device2 }
        };

        var room = new Room
        {
            Id = roomId,
            Name = "Living Room",
            Devices = devices
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 1,
            Devices = devices,
            Rooms = { room },
            HomeOwner = Guid.NewGuid()
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeDevices(homeId, roomId)).Returns(home.Devices);

        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHomeDevices(homeId, roomId);

        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        okResult.Value.Should().BeEquivalentTo(devices, options => options.WithStrictOrdering());

        homeLogic.Verify(x => x.GetHomeDevices(homeId, roomId), Times.Once);
    }

    [TestMethod]
    public void GetHomeDevicesByRoomId_ShouldReturnNoContent()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeDevices(homeId, roomId))
            .Throws(new EmptyException("No devices found for this room."));

        var controller = new HomeController(homeLogic.Object, null);

        Action act = () => controller.GetHomeDevices(homeId, roomId);

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No devices found for this room.")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        homeLogic.VerifyAll();
    }
}
