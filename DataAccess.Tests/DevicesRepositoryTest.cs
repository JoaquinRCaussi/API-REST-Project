using System.Diagnostics.CodeAnalysis;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;


[ExcludeFromCodeCoverage]
[TestClass]
public class DevicesRepositoryTest
{
    private Guid _companyId;
    private Guid _userId;
    private Guid _anotherCompanyId;
    
    private HMDbContext CreateInMemoryDbContext(string dbName)
    {
        DbContextOptions<HMDbContext>? options = new DbContextOptionsBuilder<HMDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new HMDbContext(options);
    }

    private void SeedData(HMDbContext context)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123",
            
        };
        var anoterUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123",
            
        };
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        var anotherCompany = new Company { Id = Guid.NewGuid(), Name = "anotherCompany", RUT = "Address", Owner = anoterUser };
        user.CompanyID = company.Id;
        user.Company = company;
        context.Users?.Add(user);
        context.Users?.Add(anoterUser);
        context.Companies?.Add(company);
        context.Companies?.Add(anotherCompany);
        _userId = user.Id;
        _companyId = company.Id;
        _anotherCompanyId = anotherCompany.Id;
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateDeviceTestOk()
    {
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", OwnerId = _userId};
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = _companyId, DeviceType = DeviceType.Sensor };
        
        using HMDbContext? context = CreateInMemoryDbContext("TestAddDevice");
        SeedData(context);
        
        var repository = new DeviceRepository(context);
        
        var result = repository.CreateDevice(device);

        context.SaveChanges();
        result.Should().BeEquivalentTo(device);
    }

    [TestMethod]
    public void ExistsDeviceTestOk()
    {
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = _companyId, DeviceType = DeviceType.Sensor };
        
        using HMDbContext? context = CreateInMemoryDbContext("ExistsDeviceTestOk");
        SeedData(context);
        
        var repository = new DeviceRepository(context);
        repository.CreateDevice(device);
        
        var result = repository.ExistsDevice(device.Name, device.CompanyId);
        
        result.Should().BeTrue();
    }
    
    [TestMethod]
    public void ExistsDevice_WhenDeviceDoesNotExist_ShouldReturnFalse()
    {
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = _companyId, DeviceType = DeviceType.Sensor };
        
        using HMDbContext? context = CreateInMemoryDbContext("ExistsDevice_WhenDeviceDoesNotExist_ShouldReturnFalse");
        SeedData(context);
        
        var repository = new DeviceRepository(context);
        
        var result = repository.ExistsDevice(device.Name, device.CompanyId);
        
        result.Should().BeFalse();
    }
    
    [TestMethod]
    public void ExistsDevice_ReturnsFalse_WhenDeviceNameIsTheSameButFromAnotherCompany()
    {
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = _companyId, DeviceType = DeviceType.Sensor };
        
        using HMDbContext? context = CreateInMemoryDbContext("ExistsDevice_ReturnsFalse_WhenDeviceNameIsTheSameButFromAnotherCompany");
        SeedData(context);
        
        var repository = new DeviceRepository(context);
        repository.CreateDevice(device);
        
        var result = repository.ExistsDevice(device.Name, _anotherCompanyId);
        result.Should().BeFalse();
    }
    
    [TestMethod]
    public void ExistsDevice_ReturnsFalse_WhenDeviceNameIsDifferentButFromTheSameCompany()
    {
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = _companyId, DeviceType = DeviceType.Sensor };
        
        using HMDbContext? context = CreateInMemoryDbContext("ExistsDevice_ReturnsFalse_WhenDeviceNameIsDifferentButFromTheSameCompany");
        SeedData(context);
        
        var repository = new DeviceRepository(context);
        repository.CreateDevice(device);
        
        var result = repository.ExistsDevice("anotherDevice", _companyId);
        result.Should().BeFalse();
    }
    
    [TestMethod]
    public void GetDevicesTest()
    {
        var device = new Device { Id = Guid.NewGuid(), Name = "aDevice", CompanyId = _companyId, DeviceType = DeviceType.Sensor };
        var device2 = new Device { Id = Guid.NewGuid(), Name = "anotherDevice", CompanyId = _companyId, DeviceType = DeviceType.Camera };
        
        using HMDbContext? context = CreateInMemoryDbContext("GetDevicesTest");
        SeedData(context);
        
        var repository = new DeviceRepository(context);
        repository.CreateDevice(device);
        repository.CreateDevice(device2);
        
        
        var result = repository.GetDevices(device.Name, "Company", "Sensor");
        
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

}
