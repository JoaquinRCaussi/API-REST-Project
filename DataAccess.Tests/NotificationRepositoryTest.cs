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

        // Crear permisos
        var permission = new Permission
        {
            Id = Guid.NewGuid(),
            Value = "CanGetNotifications"
        };
        
        context.Permissions.Add(permission);

        // Configurar MemberSettings
        var memberSetting1 = new MemberSetting
        {
            HomeId = home.Id,
            UserId = user1.Id,
            Permissions = new List<Permission> { permission } // Tiene el permiso
        };

        var memberSetting2 = new MemberSetting
        {
            HomeId = home.Id,
            UserId = user2.Id,
            Permissions = new List<Permission>() // No tiene el permiso
        };

        context.Users?.AddRange(user1, user2);
        context.Devices?.Add(device);
        context.Homes?.Add(home);
        context.HomeDevices?.Add(homeDevice);
        context.MemberSettings?.AddRange(memberSetting1, memberSetting2);
        context.SaveChanges();
    }

    [TestMethod]
    public void CreateNotificationSensor_ShouldCreateNotificationsOnlyForMembersWithPermission()
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
        
        var notifications = repository.CreateNotificationSensor(home.Id, homeDevice.HardwareId, sensorRequest);
        
        var notificationsInDb = context.Notifications?.ToList();

        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(1); // Solo 1 miembro tiene el permiso

        // Verificar que el usuario con permiso (user1) recibió la notificación
        notificationsInDb.Should().ContainSingle(n => home.Members != null && n.UserId == home.Members.First().Id);
        notificationsInDb.Should().OnlyContain(n => n.Event == "open");
        notificationsInDb.Should().OnlyContain(n => n.HardwareId == homeDevice.HardwareId);
        notificationsInDb.Should().OnlyContain(n => !n.IsRead);
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
        notificationsInDb.Should().HaveCount(1);
    }

    [TestMethod]
    public void CreateNotificationCameraPersonDetected_ShouldCreateNotificationsOnlyForMembersWithPermission()
    {
        using var context = CreateInMemoryDbContext("CreateNotificationCameraPersonDetectedTest");
        SeedData(context);

        var repository = new NotificationRepository(context);
        var home = context.Homes?.First();
        var homeDevice = context.HomeDevices?.First();

        var sensorRequest = new SensorRequest
        {
            Event = "personDetected"
        };
        
        var notifications = repository.CreateNotificationCamera(home.Id, homeDevice.HardwareId, sensorRequest);
        
        var notificationsInDb = context.Notifications?.ToList();

        notifications.Should().NotBeNull();
        notifications.Should().HaveCount(1);
        notificationsInDb.Should().ContainSingle(n => home.Members != null && n.UserId == home.Members.First().Id);
        notificationsInDb.Should().OnlyContain(n => n.Event == "personDetected");
        notificationsInDb.Should().OnlyContain(n => n.HardwareId == homeDevice.HardwareId);
        notificationsInDb.Should().OnlyContain(n => !n.IsRead);
    }
    
}
