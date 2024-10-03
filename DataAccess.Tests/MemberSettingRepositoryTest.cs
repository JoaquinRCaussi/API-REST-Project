using System.Diagnostics.CodeAnalysis;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class MemberSettingRepositoryTest
{
    private HMDbContext CreateInMemoryDbContext(string dbName)
    {
        DbContextOptions<HMDbContext> options = new DbContextOptionsBuilder<HMDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new HMDbContext(options);
    }

    private void SeedData(HMDbContext context)
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            MemberCount = 3,
            Devices = "TV, Fridge, Oven"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john@mail.com"
        };

        var permission = new Permission
        {
            Id = Guid.NewGuid(),
            Value = "CanGetNotifications"
        };

        var memberSetting = new MemberSetting
        {
            HomeId = home.Id,
            UserId = user.Id,
            Permissions = new List<Permission> { permission }
        };

        context.Homes?.Add(home);
        context.Users?.Add(user);
        context.Permissions?.Add(permission);
        context.MemberSettings?.Add(memberSetting);
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateMemberSetting_ShouldAddNewMemberSetting()
    {
        using var context = CreateInMemoryDbContext("CreateMemberSettingTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();

        var result = repository.CreateMemberSetting(home.Id, user.Id);

        result.Should().NotBeNull();
        result.HomeId.Should().Be(home.Id);
        result.UserId.Should().Be(user.Id);
        result.Permissions.Should().ContainSingle(p => p.Value == "CanGetNotifications");
    }

    [TestMethod]
    public void GetMemberSettings_ShouldReturnAllMemberSettings()
    {
        using var context = CreateInMemoryDbContext("GetMemberSettingsTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        repository.CreateMemberSetting((context.Homes ?? throw new InvalidOperationException()).First().Id, (context.Users ?? throw new InvalidOperationException()).First().Id);

        var result = repository.GetMemberSettings();

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetMemberSetting_ShouldReturnCorrectMemberSetting()
    {
        using var context = CreateInMemoryDbContext("GetMemberSettingTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();

        var createdSetting = repository.CreateMemberSetting(home.Id, user.Id);
        var result = repository.GetMemberSetting(home.Id, user.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdSetting);
    }

    [TestMethod]
    public void UpdateMemberSetting_ShouldUpdateExistingMemberSetting()
    {
        using var context = CreateInMemoryDbContext("UpdateMemberSettingTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();

        var createdSetting = repository.CreateMemberSetting(home.Id, user.Id);
        createdSetting.Permissions.Clear(); // Removing permissions for test
        var updatedSetting = repository.UpdateMemberSetting(createdSetting);

        updatedSetting.Permissions.Should().BeEmpty();
    }

    [TestMethod]
    public void DeleteMemberSetting_ShouldRemoveMemberSetting()
    {
        using var context = CreateInMemoryDbContext("DeleteMemberSettingTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();
        
        var createdSetting = repository.CreateMemberSetting(home.Id, user.Id);
        var result = repository.DeleteMemberSetting(createdSetting.Id);

        result.Should().BeEquivalentTo(createdSetting);
        context.MemberSettings.Should().BeEmpty();
    }

    [TestMethod]
    public void AddPermission_ShouldAddPermissionToMemberSetting()
    {
        using var context = CreateInMemoryDbContext("AddPermissionTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();

        var result = repository.AddPermission(home.Id, user.Id, "CanGetNotifications");
        
        Console.WriteLine(result.Permissions.Count);

        result.Permissions.Should().Contain(p => p.Value == "CanGetNotifications");
    }

    [TestMethod]
    public void RemovePermission_ShouldRemovePermissionFromMemberSetting()
    {
        using var context = CreateInMemoryDbContext("RemovePermissionTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();

        var memberSetting = repository.CreateMemberSetting(home.Id, user.Id);
        var updatedSetting = repository.RemovePermission(home.Id, user.Id, "CanGetNotifications");

        updatedSetting.Permissions.Should().BeEmpty();
    }

    [TestMethod]
    public void HasPermission_ShouldReturnTrueWhenPermissionExists()
    {
        using var context = CreateInMemoryDbContext("HasPermissionTest");
        SeedData(context);

        var repository = new MemberSettingRepository(context);
        var home = context.Homes?.First();
        var user = context.Users?.First();

        repository.CreateMemberSetting(home.Id, user.Id);

        var hasPermission = repository.HasPermission(home.Id, user.Id, "CanGetNotifications");

        hasPermission.Should().BeTrue();
    }
}
