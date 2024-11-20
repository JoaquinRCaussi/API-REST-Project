using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;

namespace Controllers.Tests;

[TestClass]
public class ImportDevicesControllerTest
{
    private Mock<IDeviceImportLogic> _deviceImportLogicMock;
    private ImportDevicesController _controller;

    [TestInitialize]
    public void Setup()
    {
        _deviceImportLogicMock = new Mock<IDeviceImportLogic>();
        _controller = new ImportDevicesController(_deviceImportLogicMock.Object);
    }

    [TestMethod]
    public void ImportDevices_ShouldCallLogicAndReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new ImportDevicesRequest
        {
            CompanyName = "Valid Company",
            AssemblyPath = "valid/path"
        };

        // Act
        var result = _controller.ImportDevices(request);

        // Assert
        _deviceImportLogicMock.Verify(x => x.ImportDevices("Valid Company", "valid/path"), Times.Once);
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.ActionName.Should().Be(nameof(_controller.ImportDevices));
        createdResult.Value.Should().Be("Devices Imported");
    }

    [TestMethod]
    public void ImportDevices_ShouldReturnBadRequest_WhenCompanyNameIsNull()
    {
        // Arrange
        var request = new ImportDevicesRequest
        {
            CompanyName = null,
            AssemblyPath = "valid/path"
        };

        // Act
        var result = _controller.ImportDevices(request);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Company Name and Assembly Path are required");
    }

    [TestMethod]
    public void ImportDevices_ShouldReturnBadRequest_WhenAssemblyPathIsNull()
    {
        // Arrange
        var request = new ImportDevicesRequest
        {
            CompanyName = "Valid Company",
            AssemblyPath = null
        };

        // Act
        var result = _controller.ImportDevices(request);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Company Name and Assembly Path are required");
    }
}
