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
public class DeviceControllerTest
{
    private DevicesController? _controller;
    private Mock<IDeviceLogic>? _deviceLogicMock;
    private Company? _company;
    private User? _user;

    [TestInitialize]
    public void Setup()
    {

        _deviceLogicMock = new Mock<IDeviceLogic>(MockBehavior.Strict);
        _controller = new DevicesController(_deviceLogicMock.Object);
        _user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Matias",
            LastName = "Cabrera",
            Email = "mail@.asdas.com",
            Password = "password@123",
            Company = _company
        };
        _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Logo = "logo.jpg",
            Owner = _user,
            OwnerId = _user.Id
        };
    }

    private Device CreateValidDevice()
    {
        return new Device
        {
            Name = "Dispositivo genérico",
            Company = _company,
            Description = "Descripción genérica",
            DeviceType = DeviceType.Camera,
            Model = "Model X",
            Photo = "photo1.jpg"
        };
    }

    private Camera CreateValidCamera()
    {
        return new Camera
        {
            Name = "Cámara genérica",
            Company = _company,
            Description = "Descripción genérica",
            DeviceType = DeviceType.Camera,
            Model = "Model X",
            Photo = "photo1.jpg",
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };
    }

    [TestMethod]
    public void CreateCamera_WhenAllPropertiesOK_ShouldReturnCreated()
    {
        _user.Company = _company;
        var httpContext = new DefaultHttpContext();
        httpContext.Items[0] = _user;
        _controller!.ControllerContext.HttpContext = httpContext;
        var camera = CreateValidCamera();
        var cameraRequest = new CameraRequest()
        {
            Name = "Cámara genérica",
            Description = "Descripción genérica",
            DeviceType = DeviceType.Camera,
            Model = "Model X",
            Photo = "photo1.jpg",
            Outdoors = true,
            Indoors = false,
            SupportMovementDetection = true,
            SupportPersonDetection = false
        };
        var expectedResponse = new CameraResponse(camera);

        _deviceLogicMock!
            .Setup(logic => logic.CreateCamera(It.IsAny<Camera>()))
            .Returns(camera);

        IActionResult result = _controller!.CreateCamera(cameraRequest);

        var expected = new CreatedAtActionResult(
            nameof(_controller.CreateCamera),
            nameof(DevicesController).Replace("Controller", ""),
            new { id = camera.Id },
            expectedResponse
        );

        result.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }

    [TestMethod]
    public void GetDevicesTestOk()
    {
        var listOfDevices = new List<Device>
    {
        CreateValidDevice(),
        CreateValidDevice(),
        CreateValidDevice()
    };

        var listOfDevicesResponse = listOfDevices.Select(d => new DeviceResponse(d)).ToList();

        _deviceLogicMock!
            .Setup(logic => logic.GetDevices("", "", "", DeviceType.Sensor))
            .Returns(listOfDevices);

        IActionResult result = _controller!.GetDevices("", "", "", DeviceType.Sensor, 1, 10);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        var expectedResponse = new
        {
            TotalResults = listOfDevices.Count,
            PageNumber = 1,
            PageSize = 10,
            Devices = listOfDevicesResponse
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetDevicesTypesOk()
    {
        var deviceTypes = new List<string>()
        {
            "Camera",
            "Sensor"
        };
        _deviceLogicMock.Setup(x => x.GetDevicesTypes()).Returns(deviceTypes);

        IActionResult result = _controller!.GetDevicesTypes();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(deviceTypes);
    }
    
    [TestMethod]
    public void GetDevices_ShouldReturnNoContent()
    {
        _deviceLogicMock!
            .Setup(logic => logic.GetDevices("", "", "", DeviceType.Sensor))
            .Returns(new List<Device>());

        IActionResult result = _controller!.GetDevices("", "", "", DeviceType.Sensor, 1, 10);

        result.Should().BeOfType<NoContentResult>();
    }
}
