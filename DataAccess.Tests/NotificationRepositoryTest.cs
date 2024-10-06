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

        var home = new Home
        {
            Id = Guid.NewGuid(),
            HomeOwner = user1.Id,
            Location = "Home Location",
            MemberCount = 2,
            Members = new List<User> { user1, user2 }
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            DeviceId = device.Id
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

        // Act
        var notification = repository.CreateNotificationSensor(home.Id, homeDevice.Id, sensorRequest);

        // Assert
        var notifications = context.Notifications?.Where(n => n.HardwareId == homeDevice.Id).ToList();

        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(home.Members.Count); // Verifica que se crean tantas notificaciones como miembros
        notifications.Should().OnlyContain(n => n.Event == "open"); // Verifica que el evento es correcto
        notifications.Should().OnlyContain(n => n.HardwareId == homeDevice.Id); // Verifica que el hardwareId es correcto
        notifications.Should().OnlyContain(n => !n.IsRead); // Verifica que todas las notificaciones están sin leer
    }

    
}
