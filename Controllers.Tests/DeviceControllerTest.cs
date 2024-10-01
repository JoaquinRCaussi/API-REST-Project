using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models;

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
        return new Device("Cámara Nikon", "Z50", DeviceType.Camera,"Compacta ligera, portátil y ergonómica.", "photo");
    }

    [TestMethod]
    public void CreateDevice_WhenAllPropertiesOK_ShouldReturnOk()
    {
      
        var device = CreateValidDevice();
        var deviceRequest = new DeviceRequest(device);  
        var expectedResponse = new DeviceResponse(device);  

        _deviceLogicMock!
            .Setup(logic => logic.CreateDevice(It.IsAny<Device>()))
            .Returns(device);


        IActionResult result = _controller!.CreateDevice(deviceRequest);

      
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }

}
