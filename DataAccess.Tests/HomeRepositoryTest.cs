using System.Diagnostics.CodeAnalysis;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeRepositoryTest
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
        var home = new Home { Id = Guid.NewGuid(), HomeOwner = Guid.NewGuid(), Location = "Home", MemberCount = 5, Devices = "TV, Fridge, Oven"};
        context.Homes?.Add(home);
        context.SaveChanges();
    }
    
    [TestMethod]
    public void CreateHomeTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddHome");
        SeedData(context);
        
        var repository = new HomeRepository(context);
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven"
        };
        
        Home? result = repository.CreateHome(expected);
        context.SaveChanges();
        
        result.Should().BeEquivalentTo(expected);
    }
    
    [TestMethod]
    public void GetHomesTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetHomes");
        SeedData(context);
        
        var repository = new HomeRepository(context);
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven"
        };
        var otherHome = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home2",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven"
        };
        
        Home? result = repository.CreateHome(expected);
        Home? anotherResult = repository.CreateHome(otherHome);
        context.SaveChanges();
        
        var homes = repository.GetHomes();
        homes.Should().NotBeNullOrEmpty();
        homes.Should().HaveCount(3);
        
        homes.Should().ContainEquivalentOf(expected);
        result.Should().BeEquivalentTo(expected);
        anotherResult.Should().BeEquivalentTo(otherHome);
    }
    
    [TestMethod]
    public void GetHomesByUserTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetHomesByUser");
        SeedData(context);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com"
        };
        
        var repository = new HomeRepository(context);
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = user.Id,
            Location = "Home",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven"
        };
        var otherHome = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home2",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven"
        };
        
        Home? result = repository.CreateHome(expected);
        Home? anotherResult = repository.CreateHome(otherHome);
        context.SaveChanges();
        
        var homes = repository.GetHomesByUser(user.Id);
        homes.Should().NotBeNullOrEmpty();
        homes.Should().HaveCount(1);
        
        homes.Should().ContainEquivalentOf(expected);
        result.Should().BeEquivalentTo(expected);
        anotherResult.Should().BeEquivalentTo(otherHome);
    }

    [TestMethod]
    public void AddMemberTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddMember");
        SeedData(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var member = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            LastName = "Doe",
            Email = "anothermail@gmail.com",
            Password = "password@123"
        };
        
        var repository = new HomeRepository(context);
        
        var home = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = user.Id,
            Location = "Home",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven",
            Members = new List<Guid>()
        };
        
        Home? result = repository.CreateHome(home);

        var updatedHome = repository.AddMember(home.Id, member.Id);
        context.SaveChanges();
        
        updatedHome.Should().NotBeNull();
        updatedHome.Members.Should().Contain(member.Id);
        
        result.Should().BeEquivalentTo(home);
    }

    [TestMethod]
    public void GetHomeTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetHome");
        SeedData(context);
        
        var repository = new HomeRepository(context);
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven"
        };
        
        Home? result = repository.CreateHome(expected);
        context.SaveChanges();
        
        var home = repository.GetHome(expected.Id);
        home.Should().NotBeNull();
        home.Should().BeEquivalentTo(expected);
        
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomeMembersTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetHomeMembers");
        SeedData(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mauil.com",
            Password = "password@123"
        };
        
        context.Users?.Add(user);

        var repository = new HomeRepository(context);

        var home = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = user.Id,
            Location = "Home",
            MemberCount = 5,
            Devices = "TV, Fridge, Oven",
            Members = new List<Guid> { user.Id }
        };

        Home? result = repository.CreateHome(home);
        context.SaveChanges();

        var members = repository.GetHomeMembers(home.Id);
        
        members.Should().NotBeNullOrEmpty();
        members.Should().HaveCount(1);
        members.Should().ContainEquivalentOf(user);
        
        result.Should().BeEquivalentTo(home);
    }
}
