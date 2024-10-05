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
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        user.CompanyID = company.Id;
        user.Company = company;
        context.Users?.Add(user);
        context.Companies?.Add(company);
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateDeviceTestOk()
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123",
            
        };
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        var device = new Device { Id = Guid.NewGuid(), Name = "Device", Company = company, CompanyId = company.Id, DeviceType = DeviceType.Sensor };
        user.CompanyID = company.Id;
        user.Company = company;
        
        using HMDbContext? context = CreateInMemoryDbContext("TestAddDevice");
        SeedData(context);
        
        var userRepository = new UserRepository(context);
        userRepository.CreateCompanyOwner(user);
        
        var companyRepository = new CompanyRepository(context);
        companyRepository.CreateCompany(company);
        
        var repository = new DeviceRepository(context);
        
        var result = repository.CreateDevice(device);

        context.SaveChanges();
        result.Should().BeEquivalentTo(device);
    }
}
