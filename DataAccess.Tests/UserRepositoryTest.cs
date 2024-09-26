using System.Diagnostics.CodeAnalysis;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class UserRepositoryTest
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
        var permission = new PermissionKey { Id = Guid.NewGuid(), Value = "ExamplePermission" };

        context.PermissionKeys?.Add(permission);

        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Permissions = { permission }
        };

        var homeownerRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "HomeOwner",
            Permissions = { permission }
        };

        var companyOwnerRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "CompanyOwner",
            Permissions = { permission }
        };

        context.Roles?.AddRange(adminRole, homeownerRole, companyOwnerRole);

        context.SaveChanges();
    }

    [TestMethod]
    public void CreateAdminTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddAdmin");
        SeedData(context); // Llamar al método para hacer el seed

        Role? adminRole = context.Roles?.FirstOrDefault(r => r.Name == "Admin");

        var repository = new UserRepository(context);
        var expected = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123"
        };

        // Llama al método que crea el admin y asigna el rol correspondiente
        User? result = repository.CreateAdmin(expected);
        context.SaveChanges();

        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Email, result.Email);

        User? storedAdmin = context.Users?.FirstOrDefault(a => a.Id == expected.Id);
        Assert.IsNotNull(storedAdmin);
        Assert.AreEqual(expected.Email, storedAdmin.Email);

        Assert.IsNotNull(storedAdmin.Role);
        Assert.AreEqual(adminRole.Id, storedAdmin.Role, "El usuario debe tener el rol Admin asignado.");
    }

    [TestMethod]
    public void GetUsersTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetUsers");
        var repository = new UserRepository(context);

        var expected = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Juan",
                LastName = "Perez",
                Email = "mail@mail.com",
                Password = "securePassword123"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Jose",
                LastName = "Gervasio",
                Email = "mail@mail.com",
                Password = "securePassword123"
            }
        };

        context.Users?.AddRange(expected);
        context.SaveChanges();

        List<User>? result = repository.GetUsers();

        Assert.AreEqual(expected.Count, result.Count);

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.AreEqual(expected[i].Id, result[i].Id);
            Assert.AreEqual(expected[i].Email, result[i].Email);
        }
    }

    [TestMethod]
    public void CreateCompanyOwnerTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddCompanyOwner");
        SeedData(context);

        Role? companyOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "CompanyOwner");

        var repository = new UserRepository(context);
        var expected = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123"
        };

        User? result = repository.CreateCompanyOwner(expected);
        context.SaveChanges();

        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Email, result.Email);

        Assert.IsNotNull(result.Role);
        Assert.AreEqual(companyOwnerRole.Id, result.Role, "El usuario debe tener el rol CompanyOwner asignado.");
    }

    [TestMethod]
    public void CreateHomeOwnerTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddCompanyOwner");
        SeedData(context);

        Role? homeOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "HomeOwner");

        var repository = new UserRepository(context);
        var expected = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123"
        };

        User? result = repository.CreateHomeOwner(expected);
        context.SaveChanges();

        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Email, result.Email);

        Assert.IsNotNull(result.Role);
        Assert.AreEqual(homeOwnerRole.Id, result.Role, "El usuario debe tener el rol HomeOwner asignado.");
    }

    [TestMethod]
    public void AddCompanyToCompanyOwnerTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddCompanyToCompanyOwner");
        SeedData(context);

        Role? companyOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "CompanyOwner");

        var repository = new UserRepository(context);
        var expected = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123",
            Role = companyOwnerRole.Id
        };

        var company = new Company { Id = Guid.NewGuid(), Name = "Company" };

        User? result = repository.AddCompanyToCompanyOwner(expected, company);
        context.SaveChanges();

        Assert.IsNotNull(result.Company);
        Assert.AreEqual(company.Id, result.Company);
    }

    [TestMethod]
    public void GetUser_WhenUserExists_ReturnsUserWithRoleAndCompany()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetUser");
        SeedData(context);
        var repository = new UserRepository(context);
        Role? homeOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "HomeOwner");

        var company = new Company
        {
            Id = Guid.NewGuid(), Name = "Example Company", RUT = "12345678-9", Logo = "example_logo.png"
        };

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123",
            Role = homeOwnerRole.Id,
            Company = company.Id
        };

        context.Users?.Add(expectedUser);
        context.SaveChanges();

        User? result = repository.GetUser(expectedUser.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedUser.Id, result.Id);
        Assert.AreEqual(expectedUser.Email, result.Email);
        Assert.IsNotNull(result.Role);
        Assert.AreEqual(expectedUser.Role, result.Role);
        Assert.IsNotNull(result.Company);
        Assert.AreEqual(expectedUser.Company, result.Company);
    }
}
