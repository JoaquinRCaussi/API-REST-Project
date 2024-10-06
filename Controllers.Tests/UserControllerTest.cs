using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class UserControllerTest
{
    #region Admin

    [TestMethod]
    public void CreateAdmin_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };
        // Arrange
        var admin = new AdminRequest
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password
        };
        var logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateAdmin(It.IsAny<User>())).Returns(admin.ToArgs());
        //Act 
        var controller = new AdminController(logic.Object);
        IActionResult act = controller.CreateAdmin(admin);
        var adminResponse = new AdminResponse { Name = admin.Name, LastName = admin.LastName, Email = admin.Email };
        var expected = new OkObjectResult(adminResponse);
        // Assert
        act.Should().BeEquivalentTo(expected);
    }

    #endregion

    [TestMethod]
    public void GetUsers_WhenAllPropertiesOk()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var expectedUsers = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "mail@mail.com",
                Password = "password@123"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Jane",
                LastName = "Doe",
                Email = "mail@mail.com",
                Password = "password@123"
            }
        };

        userLogicMock.Setup(logic => logic.GetUsers()).Returns(expectedUsers);

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUsers();

        var userResponses = expectedUsers.Select(u => new GetUserResponse
        {
            Email = u.Email,
            Name = u.Name,
            LastName = u.LastName
        }).ToList();

        var expectedResponse = new OkObjectResult(userResponses);

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUser_WhenAllPropertiesOk()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        userLogicMock.Setup(logic => logic.GetUser(It.IsAny<Guid>())).Returns(expectedUser);
        homeLogicMock.Setup(logic => logic.GetHomesByUser(It.IsAny<Guid>())).Returns([]);

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUser(expectedUser.Id);

        var userResponse = new GetUserResponse
        {
            Email = expectedUser.Email,
            Name = expectedUser.Name,
            LastName = expectedUser.LastName
        };

        var expectedResponse = new OkObjectResult(userResponse);

        result.Should().BeEquivalentTo(expectedResponse);
    }


    [TestMethod]
    public void DeleteAdminAccount_WhenIdIsCorrect()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "John", LastName = "Doe", Email = "mail@gmail.com" };
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        userLogicMock.Setup(logic => logic.ExistUser(user.Id)).Returns(true);
        userLogicMock.Setup(logic => logic.DeleteUser(user.Id)).Returns(user);

        var userController = new UserController(userLogicMock.Object, homeLogicMock.Object);
        var result = userController.DeleteUser(user.Id);

        var expectedResponse = new OkObjectResult(user);
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUserHomes_WhenAllPropertiesOk()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();
        var expectedHomes = new List<Home>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Home",
                HomeOwner = userId,
                Devices = [],
                Members = [],
                MemberCount = 0,
                Latitude = "asdasd",
                Longitude = "123123"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Home",
                HomeOwner = userId,
                Devices = [],
                Members = [],
                MemberCount = 0,
                Latitude = "123123",
                Longitude = "12312"
            }
        };

        homeLogicMock.Setup(logic => logic.GetHomesByUser(userId)).Returns(expectedHomes);

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUserHomes(userId);

        var homeResponses = expectedHomes.Select(h => new HomeResponse
        {
            Location = h.Location,
            HomeOwner = h.HomeOwner,
            Devices = h.Devices,
            MemberCount = h.MemberCount,
            Latitude = h.Latitude,
            Longitude = h.Longitude
        }).ToList();

        var expectedResponse = new OkObjectResult(homeResponses);

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUserHomes_WhenUserIsMemberButNotOwner()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();
        var otherOwnerId = Guid.NewGuid();

        var expectedHomes = new List<Home>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Home 1",
                HomeOwner = otherOwnerId,
                Devices = [],
                Members =
                [
                    new User { Id = userId }
                ],
                MemberCount = 1,
                Latitude = "asdasd",
                Longitude = "123123"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Home 2",
                HomeOwner = otherOwnerId,
                Devices = [],
                Members =
                [
                    new User { Id = userId }
                ],
                MemberCount = 1,
                Latitude = "123123",
                Longitude = "12312"
            }
        };

        homeLogicMock.Setup(logic => logic.GetHomesByUser(userId)).Returns(expectedHomes);

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUserHomes(userId);

        var homeResponses = expectedHomes.Select(h => new HomeResponse
        {
            Location = h.Location,
            HomeOwner = h.HomeOwner,
            Devices = h.Devices,
            MemberCount = h.MemberCount,
            Latitude = h.Latitude,
            Longitude = h.Longitude
        }).ToList();

        var expectedResponse = new OkObjectResult(homeResponses);

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUserNotifications_WhenNotificationsExist()
    {
        // Arrange
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();

        var expectedNotifications = new List<Notification>
        {
            new Notification { Id = Guid.NewGuid(), Event = "Notification 1", UserId = userId, CreatedAt = DateTime.UtcNow },
            new Notification { Id = Guid.NewGuid(), Event = "Notification 2", UserId = userId, CreatedAt = DateTime.UtcNow }
        };

        userLogicMock.Setup(logic => logic.GetNotifications(userId)).Returns(expectedNotifications);

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUserNotifications(userId);

        var expectedResponse = new OkObjectResult(expectedNotifications);
        result.Should().BeEquivalentTo(expectedResponse);

        userLogicMock.Verify(logic => logic.GetNotifications(userId), Times.Once);
    }

    [TestMethod]
    public void GetUserNotifications_WhenNotificationsDoNotExist()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();

        userLogicMock.Setup(logic => logic.GetNotifications(userId)).Returns([]);

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUserNotifications(userId);

        var expectedResponse = new OkObjectResult(new List<Notification>());
        result.Should().BeEquivalentTo(expectedResponse);

        userLogicMock.Verify(logic => logic.GetNotifications(userId), Times.Once);
    }

}
