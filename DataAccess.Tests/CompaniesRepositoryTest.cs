using System.Diagnostics.CodeAnalysis;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class CompaniesRepositoryTest
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
        var user = new User() { Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"};
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        context.Companies?.Add(company);
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateCompanyTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        
        using HMDbContext? context = CreateInMemoryDbContext("TestAddCompany");
        SeedData(context);
        var repository = new CompanyRepository(context);
        
        var result = repository.CreateCompany(company);
        
        context.SaveChanges();
        result.Should().BeEquivalentTo(company);
    }
    
    [TestMethod]
    public void GetCompaniesTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetCompanies");
        SeedData(context);
        var repository = new CompanyRepository(context);
        
        var result = repository.GetCompanies("Company", "John");
        
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }
}
