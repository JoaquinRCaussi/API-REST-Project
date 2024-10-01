using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
//using WebApi.Controllers;
using WebApi.Models;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class CameraControllerTest
{
    private CameraController? _controller;
    private Mock<IDeviceLogic>? _cameraLogicMock;

    [TestInitialize]
    public void Setup()
    {
        _cameraLogicMock = new Mock<IDeviceLogic>();
        _controller = new CameraController(_cameraLogicMock.Object);
    }

    private Camera CreateValidCamera()
    {
        return new Camera(
            "Cámara Nikon",
            "Z50",
            "Cámara compacta y ligera.",
            "photo1.jpg",
            DeviceType.Camera,
            outdoors: true,
            indoors: false,
            supportMovementDetection: true,
            supportPersonDetection: true
        );
    }

    [TestMethod]
    public void CreateCamera_WhenAllPropertiesOK_ShouldReturnOk()
    {
        // Arrange
        var camera = CreateValidCamera();
        var cameraRequest = new CameraRequest(camera);
        var expectedResponse = new CameraResponse(camera);

        _cameraLogicMock!
            .Setup(logic => logic.CreateCamera(It.IsAny<Camera>()))
            .Returns(camera);

        // Act
        IActionResult result = _controller!.CreateCamera(cameraRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
