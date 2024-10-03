using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Models;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class DeviceControllerTest
{
    private DeviceController? _controller;
    private Mock<IDeviceLogic>? _deviceLogicMock;

    [TestInitialize]
    public void Setup()
    {
        _deviceLogicMock = new Mock<IDeviceLogic>();
        _controller = new DeviceController(_deviceLogicMock.Object);
    }

    private Device CreateValidDevice()
    {
        return new Device
        {
            Name = "Dispositivo genérico",
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
        // Arrange
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
}
