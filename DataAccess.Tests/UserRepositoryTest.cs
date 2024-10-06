using System.Diagnostics.CodeAnalysis;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
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

        var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin" };

        var homeownerRole = new Role { Id = Guid.NewGuid(), Name = "HomeOwner" };

        var companyOwnerRole = new Role { Id = Guid.NewGuid(), Name = "CompanyOwner" };

        context.Roles?.AddRange(adminRole, homeownerRole, companyOwnerRole);

        context.SaveChanges();
    }

    [TestMethod]
    public void CreateAdminTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddAdmin");
        SeedData(context);

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
        
        User? result = repository.CreateAdmin(expected);
        context.SaveChanges();

        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Email, result.Email);

        User? storedAdmin = context.Users?.FirstOrDefault(a => a.Id == expected.Id);
        Assert.IsNotNull(storedAdmin);
        Assert.AreEqual(expected.Email, storedAdmin.Email);

        Assert.IsNotNull(storedAdmin.Role);
        Assert.AreEqual(adminRole.Id, storedAdmin.RoleID, "El usuario debe tener el rol Admin asignado.");
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
        Assert.AreEqual(companyOwnerRole.Id, result.RoleID, "El usuario debe tener el rol CompanyOwner asignado.");
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
        Assert.AreEqual(homeOwnerRole.Id, result.RoleID, "El usuario debe tener el rol HomeOwner asignado.");
    }

    [TestMethod]
    public void GetUser_WhenUserExists_ReturnsUserWithRoleAndCompany()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetUser");
        SeedData(context);
        var repository = new UserRepository(context);
        Role? homeOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "HomeOwner");

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123",
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Example Company",
            RUT = "12345678-9",
            Logo = "example_logo.png",
            Owner = expectedUser
        };

        expectedUser.Company = company;
        expectedUser.CompanyID = company.Id;

        context.Users?.Add(expectedUser);
        context.SaveChanges();

        User? result = repository.GetUser(expectedUser.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUser);
    }

    [TestMethod]
    public void GetUser_WhenUserDoesNotExist_ReturnsNull()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetUser");
        SeedData(context);
        var repository = new UserRepository(context);

        User? result = repository.GetUser(Guid.NewGuid());

        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetUserByEmail_WhenUserExists_ReturnsUserWithRoleAndCompany()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetUserByEmail");
        SeedData(context);
        var repository = new UserRepository(context);
        Role? homeOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "HomeOwner");

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "mail@mail.com",
            Password = "securePassword123",
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Example Company",
            RUT = "12345678-9",
            Logo = "example_logo.png",
            Owner = expectedUser
        };

        expectedUser.RoleID = homeOwnerRole.Id;
        expectedUser.CompanyID = company.Id;

        context.Users?.Add(expectedUser);

        context.SaveChanges();

        User? result = repository.FindByMail(expectedUser.Email);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUser);
    }

    [TestMethod]
    public void AuthenticateUser_WhenCredentialsAreCorrect_ReturnsUser()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAuthenticateUser");
        SeedData(context);

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "user@example.com",
            Password = "securePassword123"
        };

        context.Users?.Add(expectedUser);
        context.SaveChanges();

        var repository = new UserRepository(context);

        User? result = repository.AuthenticateUser("user@example.com", "securePassword123");

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUser);
    }

    [TestMethod]
    public void AuthenticateUser_WhenEmailIsIncorrect_ReturnsNull()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAuthenticateUserInvalidEmail");
        SeedData(context);

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "user@example.com",
            Password = "securePassword123"
        };

        context.Users?.Add(expectedUser);
        context.SaveChanges();

        var repository = new UserRepository(context);

        User? result = repository.AuthenticateUser("wronguser@example.com", "securePassword123");

        result.Should().BeNull();
    }

    [TestMethod]
    public void AuthenticateUser_WhenPasswordIsIncorrect_ReturnsNull()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAuthenticateUserInvalidPassword");
        SeedData(context);

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            LastName = "Perez",
            Email = "user@example.com",
            Password = "securePassword123"
        };

        context.Users?.Add(expectedUser);
        context.SaveChanges();

        var repository = new UserRepository(context);

        User? result = repository.AuthenticateUser("user@example.com", "wrongPassword");

        result.Should().BeNull();
    }

    [TestMethod]
    public void ExistUser_WhenUserExists_ReturnsTrue()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestExistUser");
        var repository = new UserRepository(context);

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password"
        };

        context.Users?.Add(existingUser);
        context.SaveChanges();
        
        var result = repository.ExistUser(existingUser.Id);
        
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ExistUser_WhenUserDoesNotExist_ReturnsFalse()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestExistUser_NotFound");
        var repository = new UserRepository(context);
        
        var result = repository.ExistUser(Guid.NewGuid());

        result.Should().BeFalse();
    }

    [TestMethod]
    public void DeleteUser_WhenUserExists_DeletesAndReturnsUser()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestDeleteUser");
        var repository = new UserRepository(context);

        var userToDelete = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Password = "password"
        };

        context.Users?.Add(userToDelete);
        context.SaveChanges();

        var result = repository.DeleteUser(userToDelete.Id);
        var userInDb = context.Users?.FirstOrDefault(u => u.Id == userToDelete.Id);
        
        result.Should().BeEquivalentTo(userToDelete);
        userInDb.Should().BeNull(); // Verifies that the user was deleted
    }

    [TestMethod]
    public void DeleteUser_WhenUserDoesNotExist_ReturnsNull()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestDeleteUser_NotFound");
        var repository = new UserRepository(context);
        
        var result = repository.DeleteUser(Guid.NewGuid());
        
        result.Should().BeNull();
    }

    [TestMethod]
    public void GetNotifications_WhenUserHasNotifications_ReturnsNotifications()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetNotifications");
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "securePassword123"
        };

        var notification1 = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "Notification 1",
            UserId = user.Id,
            User = user
        };
        
        var notification2 = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "Notification 2",
            UserId = user.Id,
            User = user
        };
        
        context.Notifications?.AddRange(notification1, notification2);
        context.SaveChanges();
        
        var result = repository.GetNotifications(user.Id);
        
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(notification1);
    }

    [TestMethod]
    public void GetNotifications_WhenUserHasNoNotifications_ReturnsEmptyList()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetNotifications_NoNotifications");
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "securePassword123"
        };

        var result = repository.GetNotifications(user.Id);
        
        result.Should().BeEmpty();
    }
}
