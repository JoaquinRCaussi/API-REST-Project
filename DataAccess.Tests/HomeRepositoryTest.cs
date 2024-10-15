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
    private Company? _company;

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
            Name = "Matias",
            LastName = "Cabrera",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };
        var _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Owner = user
        };
        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };
        var home = new Home { Id = Guid.NewGuid(), HomeOwner = Guid.NewGuid(), Location = "Home", MemberCount = 5, Devices = homeDevices, Latitude = "123", Longitude = "123" };
        context.Homes?.Add(home);
        context.SaveChanges();
    }

    [TestInitialize]
    public void Setup()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Matias",
            LastName = "Cabrera",
            Email = "mail@.asdas.com",
            Password = "password@123"
        };
        _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Owner = user
        };
    }

    [TestMethod]
    public void CreateHomeTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestAddHome");
        SeedData(context);

        var repository = new HomeRepository(context);
        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
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

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Name = "device", Company = _company, Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };
        var repository = new HomeRepository(context);
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
        };
        var otherHome = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home2",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
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

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

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
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
        };
        var otherHome = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home2",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
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

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

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
        var userRepository = new UserRepository(context);

        userRepository.CreateHomeOwner(member);
        userRepository.CreateHomeOwner(user);
        context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = user.Id,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices,
            Members = []
        };

        Home? result = repository.CreateHome(home);

        var updatedHome = repository.AddMember(home.Id, member.Id);
        context.SaveChanges();

        updatedHome.Should().NotBeNull();
        updatedHome.Members.Should().NotBeNullOrEmpty();
        updatedHome.Members.Should().HaveCount(1);
        updatedHome.Members.Should().ContainEquivalentOf(member);

        result.Should().BeEquivalentTo(home);
    }

    [TestMethod]
    public void GetHomeTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestGetHome");
        SeedData(context);

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Name = "device", Company = _company, Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var repository = new HomeRepository(context);
        var expected = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
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

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };
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
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices,
            Members = [user]
        };

        Home? result = repository.CreateHome(home);
        context.SaveChanges();

        var members = repository.GetHomeMembers(home.Id);

        members.Should().NotBeNullOrEmpty();
        members.Should().HaveCount(1);
        members.Should().ContainEquivalentOf(user);

        result.Should().BeEquivalentTo(home);
    }

    [TestMethod]
    public void AddMember_ShouldReturnDefaultHome_WhenHomeDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("TestAddMemberHomeNull");
        var repository = new HomeRepository(context);

        var nonExistentHomeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var result = repository.AddMember(nonExistentHomeId, userId);

        result.Should().NotBeNull();
        result.Location.Should().BeNull();
        result.MemberCount.Should().Be(0);
        result.Devices.Should().BeNull();
        result.HomeOwner.Should().Be(default(Guid));
    }

    [TestMethod]
    public void GetHomeMembers_ShouldReturnEmptyList_WhenHomeIsNullOrHasNoMembers()
    {
        using var context = CreateInMemoryDbContext("TestGetHomeMembersNullOrEmpty");
        var repository = new HomeRepository(context);

        var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Company = _company, Name = "device", Model = "model", DeviceType = DeviceType.Camera, Description = "description", Photo = "photo" } };
        var homeDevices = new List<HomeDevice> { new HomeDevice { Id = Guid.NewGuid(), DeviceId = devices[0].Id } };

        var nonExistentHomeId = Guid.NewGuid();

        var homeIdWithNoMembers = Guid.NewGuid();
        var homeWithNoMembers = new Home
        {
            Id = homeIdWithNoMembers,
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = homeDevices
        };

        context.Homes?.Add(homeWithNoMembers);
        context.SaveChanges();

        var resultForNullHome = repository.GetHomeMembers(nonExistentHomeId);

        var resultForEmptyMembers = repository.GetHomeMembers(homeIdWithNoMembers);

        resultForNullHome.Should().NotBeNull();
        resultForNullHome.Should().BeEmpty();

        resultForEmptyMembers.Should().NotBeNull();
        resultForEmptyMembers.Should().BeEmpty();
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnEmptyList_WhenHomeDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("TestGetHomeDevicesHomeNull");
        var repository = new HomeRepository(context);

        var nonExistentHomeId = Guid.NewGuid();

        var result = repository.GetHomeDevices(nonExistentHomeId);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void GetHomeDevices_ShouldReturnListOfDevices_WhenHomeExists()
    {
        using var context = CreateInMemoryDbContext("TestGetHomeDevicesHomeExists");
        var repository = new HomeRepository(context);

        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Owner = user
        };

        var device = new Device
        {
            Id = deviceId,
            Company = _company,
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "description",
            Photo = "photo"
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            Device = device
        };

        var home = new Home
        {
            Id = homeId,
            HomeOwner = Guid.NewGuid(),
            MemberCount = 4,
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            Devices = [homeDevice]
        };

        context.Homes?.Add(home);
        context.Devices?.Add(device);
        context.HomeDevices?.Add(homeDevice);
        context.SaveChanges();

        var result = repository.GetHomeDevices(homeId);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].DeviceId.Should().Be(deviceId);
        result[0].Device.Should().Be(device);
    }

    [TestMethod]
    public void AddDevice_ShouldReturnDefaultHomeDevice_WhenHomeOrDeviceDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("TestAddDeviceHomeOrDeviceNull");
        var repository = new HomeRepository(context);

        var nonExistentHomeId = Guid.NewGuid();
        var nonExistentDeviceId = Guid.NewGuid();

        var result = repository.AddDevice(nonExistentHomeId, nonExistentDeviceId);

        result.Should().NotBeNull();
        result.DeviceId.Should().Be(default(Guid));
        result.Device.Should().BeNull();
    }

    [TestMethod]
    public void AddDevice_ShouldAddDeviceToHome_WhenHomeAndDeviceExist()
    {
        using var context = CreateInMemoryDbContext("TestAddDeviceHomeAndDeviceExist");
        var repository = new HomeRepository(context);

        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var home = new Home
        {
            Id = homeId,
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = []
        };

        var _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Owner = user
        };

        var device = new Device
        {
            Id = deviceId,
            Company = _company,
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "description",
            Photo = "photo"
        };

        context.Homes?.Add(home);
        context.Devices?.Add(device);
        context.SaveChanges();

        var result = repository.AddDevice(homeId, deviceId);

        result.Should().NotBeNull();
        result.DeviceId.Should().Be(deviceId);
        result.Device.Should().Be(device);
        home.Devices.Should().Contain(result);
    }
    
    [TestMethod]
    public void ChangeHomeDeviceStatus_ShouldReturnDefaultHomeDevice_WhenHomeOrDeviceDoesNotExist()
    {
        using var context = CreateInMemoryDbContext("TestChangeHomeDeviceStatusHomeOrDeviceNull");
        var repository = new HomeRepository(context);

        var nonExistentHomeId = Guid.NewGuid();
        var nonExistentDeviceId = Guid.NewGuid();

        var result = repository.ChangeHomeDeviceStatus(nonExistentHomeId, nonExistentDeviceId, true);

        result.Should().NotBeNull();
        result.DeviceId.Should().Be(default(Guid));
        result.Device.Should().BeNull();
    }

    [TestMethod]
    public void ChangeHomeDeviceStatus_ShouldChangeDeviceStatus_WhenHomeAndDeviceExist()
    {
        using var context = CreateInMemoryDbContext("TestChangeHomeDeviceStatusHomeAndDeviceExist");
        var repository = new HomeRepository(context);

        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var home = new Home
        {
            Id = homeId,
            HomeOwner = Guid.NewGuid(),
            Location = "Home",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            Devices = []
        };
        
        var _company = new Company()
        {
            Id = Guid.NewGuid(),
            Name = "anotherCompany",
            RUT = "2312311",
            Owner = user
        };
        
        var device = new Device
        {
            Id = deviceId,
            Company = _company,
            Name = "Device",
            Model = "Model",
            DeviceType = DeviceType.Camera,
            Description = "description",
            Photo = "photo"
        };
        
        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            HardwareId = Guid.NewGuid(),
            DeviceId = deviceId,
            Device = device,
            State = false
        };
        
        context.Homes?.Add(home);
        context.Devices?.Add(device);
        context.HomeDevices?.Add(homeDevice);
        
        context.SaveChanges();
        
        var result = repository.ChangeHomeDeviceStatus(homeId, deviceId, true);
    }
}
