using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Entities;
using DataAccess.Data;
using DataAccess.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class DeviceRepositoryTest
{
    private HMDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<HMDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new HMDbContext(options);
    }

    private void SeedData(HMDbContext context)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123",
        };
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        user.CompanyID = company.Id;

        context.Users.Add(user);
        context.Companies.Add(company);
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateDevice_ShouldAddDevice_WhenCompanyExists()
    {
        using var context = CreateInMemoryDbContext("CreateDeviceTest");
        SeedData(context);

        var repository = new DeviceRepository(context);
        if (context.Companies != null)
        {
            var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = context.Companies.First().Id, DeviceType = DeviceType.WindowSensor };

            var result = repository.CreateDevice(device);

            context.Devices.Should().ContainSingle();
            result.Should().BeEquivalentTo(device);
        }
    }

    [TestMethod]
    public void CreateDevice_ShouldThrowException_WhenCompanyDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("CreateDeviceTest_CompanyDoesNotExist");

        var repository = new DeviceRepository(context);
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = Guid.NewGuid(), DeviceType = DeviceType.WindowSensor };

        Action act = () => repository.CreateDevice(device);

        act.Should().Throw<Exception>().WithMessage("The Company does not exist");
    }

    [TestMethod]
    public void CreateCamera_ShouldAddCamera_WhenCompanyExists()
    {
        using var context = CreateInMemoryDbContext("CreateCameraTest");
        SeedData(context);

        var repository = new DeviceRepository(context);
        if (context.Companies != null)
        {
            var camera = new Camera { Id = Guid.NewGuid(), Name = "aCamera", CompanyId = context.Companies.First().Id, DeviceType = DeviceType.Camera };

            var result = repository.CreateCamera(camera);

            context.Devices.Should().ContainSingle();
            result.Should().BeEquivalentTo(camera);
        }
    }

    [TestMethod]
    public void CreateCamera_ShouldThrowException_WhenCompanyDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("CreateCameraTest_CompanyDoesNotExist");

        var repository = new DeviceRepository(context);
        var camera = new Camera { Id = Guid.NewGuid(), Name = "aCamera", CompanyId = Guid.NewGuid(), DeviceType = DeviceType.Camera };

        Action act = () => repository.CreateCamera(camera);

        act.Should().Throw<Exception>().WithMessage("The Company does not exist");
    }

    [TestMethod]
    public void ExistsDevice_ShouldReturnTrue_WhenDeviceExists()
    {
        using var context = CreateInMemoryDbContext("ExistsDeviceTest");
        SeedData(context);

        var repository = new DeviceRepository(context);
        if (context.Companies != null)
        {
            var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = context.Companies.First().Id, DeviceType = DeviceType.WindowSensor };
            repository.CreateDevice(device);

            var result = repository.ExistsDevice(device.Name, device.CompanyId);

            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void ExistsDevice_ShouldReturnFalse_WhenDeviceDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("ExistsDeviceDoesNotExistTest");
        SeedData(context);

        var repository = new DeviceRepository(context);

        var result = context.Companies != null && repository.ExistsDevice("NonExistentDevice", context.Companies.First().Id);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void GetDevices_ShouldReturnDevices_WhenDevicesExist()
    {
        using var context = CreateInMemoryDbContext("GetDevicesTest");
        SeedData(context);

        var repository = new DeviceRepository(context);
        if (context.Companies != null)
        {
            var device1 = new Device { Id = Guid.NewGuid(), Name = "aDevice", Model = "Model1", CompanyId = context.Companies.First().Id, DeviceType = DeviceType.WindowSensor };
            var device2 = new Device { Id = Guid.NewGuid(), Name = "anotherDevice", Model = "Model2", CompanyId = context.Companies.First().Id, DeviceType = DeviceType.Camera };
            repository.CreateDevice(device1);
            repository.CreateDevice(device2);

            var result = repository.GetDevices("aDevice", "", "Company", DeviceType.WindowSensor);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(device1);
        }
    }

    [TestMethod]
    public void GetDevicesNoType_ShouldReturnDevices_WhenDevicesExist()
    {
        using var context = CreateInMemoryDbContext("GetDevicesNoTypeTest");
        SeedData(context);

        var repository = new DeviceRepository(context);
        if (context.Companies != null)
        {
            var device1 = new Device { Id = Guid.NewGuid(), Name = "aDevice", Model = "Model1", CompanyId = context.Companies.First().Id };
            var device2 = new Device { Id = Guid.NewGuid(), Name = "anotherDevice", Model = "Model2", CompanyId = context.Companies.First().Id };
            repository.CreateDevice(device1);
            repository.CreateDevice(device2);

            var result = repository.GetDevicesNoType("aDevice", "Model1", "Company");

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(device1);
        }
    }
}
