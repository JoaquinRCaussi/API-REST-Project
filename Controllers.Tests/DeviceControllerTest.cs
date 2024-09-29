using System.Diagnostics.CodeAnalysis;
using Domain;
//using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class DeviceControllerTest
{
    [TestMethod]
    public void CreateDevice_WhenAllPropertiesOK()
    {
        var device = new Device("Cámara Nikon", "Z50", "Compacta ligera, portátil y ergonómica.", "photo");

        // Mocking IDeviceLogic to simulate the behavior of the business logic
        var deviceLogicMock = new Mock<IDeviceLogic>();
        deviceLogicMock
            .Setup(logic => logic.CreateDevice(It.IsAny<Device>()))
            .Returns(device);  // Simulate that the logic returns the same device

        // Mock DeviceController passing the mocked logic
        var controller = new DeviceController(deviceLogicMock.Object);

        // Act
        IActionResult result = controller.CreateDevice(device);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));  // Expecting an Ok result
        var okResult = result as OkObjectResult;

        // Check if the returned device is the same that we passed
        Assert.IsNotNull(okResult);
        Assert.AreEqual(device, okResult.Value);
    }

    [TestMethod]
    public void CreateDevice_WhenPropertiesMissing_ShouldReturnBadRequest()
    {
        // Arrange
        var device2 = new Device("Cámara Nikon", null, "Compacta ligera, portátil y ergonómica.", "photo");

        var deviceLogicMock = new Mock<IDeviceLogic>();
        var controller = new DeviceController(deviceLogicMock.Object);

        // Act
        IActionResult result = controller.CreateDevice(device2);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }



}
