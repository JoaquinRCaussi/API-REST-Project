using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

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
        return new Device("Cámara Nikon", "Z50", "Compacta ligera, portátil y ergonómica.", "photo");
    }

    private Device CreateInvalidDevice()
    {
        return new Device("Cámara Nikon", null, "Compacta ligera, portátil y ergonómica.", "photo");
    }

    [TestMethod]
    public void CreateDevice_WhenAllPropertiesOK()
    {
        var device = CreateValidDevice();

        _deviceLogicMock!
            .Setup(logic => logic.CreateDevice(It.IsAny<Device>()))
            .Returns(device);

        IActionResult result = _controller!.CreateDevice(device);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(device);
    }

    [TestMethod]
    public void CreateDevice_WhenPropertiesMissing_ShouldReturnBadRequest()
    {
        var device = CreateInvalidDevice();

        IActionResult result = _controller!.CreateDevice(device);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
