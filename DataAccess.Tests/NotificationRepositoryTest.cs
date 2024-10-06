using DataAccess.Data;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Tests;

[TestClass]
public class NotificationRepositoryTest
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
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john@mail.com"
        };
    
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            LastName = "Doe",
            Email = "jane@mail.com"
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Company",
            RUT = "2312311",
            Owner = user1
        };

        var device = new Device
        {
            Id = Guid.NewGuid(),
            Company = company,
            Name = "Camera",
            Model = "Model X",
            DeviceType = DeviceType.Camera,
            Description = "A security camera",
            Photo = "photo_url"
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            DeviceId = device.Id
        };

        
        var home = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = user1.Id,
            Location = "Home Location",
            MemberCount = 2,
            Devices = new List<HomeDevice> { homeDevice },
            Members = new List<User> { user1, user2 }
        };


        context.Users?.AddRange(user1, user2);
        context.Devices?.Add(device);
        context.Homes?.Add(home);
        context.HomeDevices?.Add(homeDevice);
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateNotificationSensor_ShouldCreateNotificationsForAllMembers()
    {
        using var context = CreateInMemoryDbContext("CreateNotificationSensorTest");
        SeedData(context);

        var repository = new NotificationRepository(context);
        var home = context.Homes?.First();
        var homeDevice = context.HomeDevices?.First();

        var sensorRequest = new SensorRequest
        {
            Event = "open"
        };
        
        var notification = repository.CreateNotificationSensor(home.Id, homeDevice.HardwareId, sensorRequest);
        
        var notifications = context.Notifications?.Where(n => n.HardwareId == homeDevice.HardwareId).ToList();

        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(home.Members.Count);
        notifications.Should().OnlyContain(n => n.Event == "open");
        notifications.Should().OnlyContain(n => n.HardwareId == homeDevice.HardwareId);
        notifications.Should().OnlyContain(n => !n.IsRead);
    }
    
    [TestMethod]
    [ExpectedException(typeof(Exception), "Home or device not found")]
    public void CreateNotificationSensor_ShouldThrowExceptionWhenHomeOrDeviceNotFound()
    {
        using var context = CreateInMemoryDbContext("CreateNotificationSensorErrorTest");
        SeedData(context);

        var repository = new NotificationRepository(context);
        var nonExistentHomeId = Guid.NewGuid();
        var homeDevice = context.HomeDevices?.First();

        var sensorRequest = new SensorRequest
        {
            Event = "open"
        };
        
        repository.CreateNotificationSensor(nonExistentHomeId, homeDevice.HardwareId, sensorRequest);
    }

    [TestMethod]
    public void CreateNotificationSensor_ShouldAddNotificationsToDatabase()
    {
        using var context = CreateInMemoryDbContext("CreateNotificationSensorAddTest");
        SeedData(context);

        var repository = new NotificationRepository(context);
        var home = context.Homes?.First();
        var homeDevice = context.HomeDevices?.First();

        var sensorRequest = new SensorRequest
        {
            Event = "open"
        };
        
        repository.CreateNotificationSensor(home.Id, homeDevice.HardwareId, sensorRequest);

        var notificationsInDb = context.Notifications?.ToList();
        notificationsInDb.Should().NotBeNull();
        notificationsInDb.Should().HaveCount(home.Members.Count);
    }


    
}
