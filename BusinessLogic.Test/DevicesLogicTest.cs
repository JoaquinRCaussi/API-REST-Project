using Domain;
using FluentAssertions;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class DevicesLogicTest 
{
    [TestMethod]
    public void CreateDeviceTest_WhenAllPropertiesAreOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Company",
            RUT = "Address",
            Logo = "Logo",
            Owner = user
        };
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "Description",
            Photo = "Photo",
            Company = company
        };

        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.CreateDevice(device)).Returns(device);

        var deviceLogic = new DeviceLogic(deviceRepository.Object);

        var result = deviceLogic.CreateDevice(device);

        result.Should().BeEquivalentTo(device);
    }
    
}
