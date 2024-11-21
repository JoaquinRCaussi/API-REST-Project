using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Entities;
using DataAccess.Data;
using DataAccess.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class SessionRepositoryTests
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
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123"
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Test Company",
            Owner = user
        };

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            PermissionKeys = new List<PermissionKey> { new PermissionKey { Value = "MANAGE_USERS" } }
        };

        var session = new Session
        {
            Token = Guid.NewGuid(),
            User = user,
            RoleID = user.RoleID
        };

        context.Users?.Add(user);
        context.Companies?.Add(company);
        context.Roles?.Add(role);
        context.Sessions?.Add(session);
        context.SaveChanges();
    }

    [TestMethod]
    public void AddSession_ShouldAddSessionToDatabase()
    {
        using HMDbContext context = CreateInMemoryDbContext("TestAddSession");
        var repository = new SessionRepository(context);

        var user = new User { Id = Guid.NewGuid(), Name = "Alice", LastName = "Smith", Email = "alice@example.com" };
        var session = new Session
        {
            Token = Guid.NewGuid(),
            User = user
        };

        repository.AddSession(session);

        var addedSession = context.Sessions?.FirstOrDefault(s => s.Token == session.Token);
        addedSession.Should().NotBeNull();
        addedSession.Should().BeEquivalentTo(session);
    }
    
}
