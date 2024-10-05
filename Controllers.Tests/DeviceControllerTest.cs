using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using SQLitePCL;
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
    public void CreateDevice_WhenAllPropertiesOK_ShouldReturnOk()
    {
        // Arrange
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
        var httpContext = new DefaultHttpContext();
        httpContext.Items[0] = _user;
        _controller!.ControllerContext.HttpContext = httpContext;
        var device = CreateValidDevice();
        var deviceRequest = new DeviceRequest(device);
        var expectedResponse = new DeviceResponse(device);

        _deviceLogicMock!
            .Setup(logic => logic.CreateDevice(It.IsAny<Device>()))
            .Returns(device);

        // Act
        IActionResult result = _controller!.CreateDevice(deviceRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void CreateCamera_WhenAllPropertiesOK_ShouldReturnOk()
    {
        _user.Company = _company;
        var httpContext = new DefaultHttpContext();
        httpContext.Items[0] = _user;
        _controller!.ControllerContext.HttpContext = httpContext;
        var camera = CreateValidCamera();
        var cameraRequest = new CameraRequest(camera);
        var expectedResponse = new CameraResponse(camera);

        _deviceLogicMock!
            .Setup(logic => logic.CreateCamera(It.IsAny<Camera>()))
            .Returns(camera);

        // Act
        IActionResult result = _controller!.CreateCamera(cameraRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetDevicesTestOk()
    {
        var ListOfDevices = new List<Device>
        {
            CreateValidDevice(),
            CreateValidDevice(),
            CreateValidDevice()
        };
        var ListOfDevicesResponse = ListOfDevices.Select(d => new DeviceResponse(d)).ToList();
        _deviceLogicMock!
            .Setup(logic => logic.GetDevices("", "", ""))
            .Returns(ListOfDevices);
        
        IActionResult result = _controller!.GetDevices("", "", "");
        
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(ListOfDevicesResponse);
    }

    [TestMethod]
    public void GetDevicesTypesOk()
    {
        List<string> deviceTypes = new List<string>
        {
            "Camera",
            "Sensor"
        };
        _deviceLogicMock.Setup(x => x.GetDevicesTypes()).Returns(deviceTypes);
        
        IActionResult result = _controller!.GetDevicesTypes();
        
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(deviceTypes);
    }
}
