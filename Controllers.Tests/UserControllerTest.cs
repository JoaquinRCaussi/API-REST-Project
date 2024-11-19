using System.Diagnostics.CodeAnalysis;
using System.Net;
using BusinessLogic;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using WebApi.Controllers;
using WebApi.Filters;
using WebApi.Models.In;
using WebApi.Models.Out;

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

        var admin = new AdminRequest
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password
        };

        var logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateAdmin(It.IsAny<User>())).Returns(user);

        var controller = new AdminController(logic.Object);
        IActionResult act = controller.CreateAdmin(admin);

        var adminResponse = new AdminResponse { Name = admin.Name, LastName = admin.LastName, Email = admin.Email };

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateAdmin),
            nameof(AdminController).Replace("Controller", ""),
            new { id = user.Id },
            adminResponse
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
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
            Email = "john.doe@mail.com",
            Password = "password@123"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            LastName = "Doe",
            Email = "jane.doe@mail.com",
            Password = "password@123"
        }
    };

        userLogicMock.Setup(logic => logic.GetUsersFiltered(null, null, 1, 10)).Returns((expectedUsers, expectedUsers.Count));

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        IActionResult result = controller.GetUsers(null, null, 1, 10);

        var paginatedUsers = expectedUsers
            .Skip(0)
            .Take(10)
            .Select(u => new GetUserResponse
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                Role = u.Role
            })
            .ToList();

        var expectedResponse = new OkObjectResult(new
        {
            TotalResults = expectedUsers.Count,
            PageNumber = 1,
            PageSize = 10,
            Users = paginatedUsers
        });

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUsers_ShouldReturnNoContent_WhenNoUsersFound()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        userLogicMock.Setup(logic => logic.GetUsersFiltered(null, null, 1, 10))
            .Throws(new EmptyException("No users found"));

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        Action act = () => controller.GetUsers(null, null, 1, 10);

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No users found")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        userLogicMock.VerifyAll();
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
            LastName = expectedUser.LastName,
            Role = expectedUser.Role,
            CreatedAt = expectedUser.CreatedAt
        };

        var expectedResponse = new OkObjectResult(userResponse);

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUserById_ShouldReturnNoContent()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();

        userLogicMock.Setup(logic => logic.GetUser(userId))
            .Throws(new NotValidDataException("User does not exist"));

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        Action act = () => controller.GetUser(userId);

        act.Should().Throw<NotValidDataException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new NotValidDataException("User does not exist")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        userLogicMock.VerifyAll();
    }


    [TestMethod]
    public void DeleteAdminAccount_WhenIdIsCorrect()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "John", LastName = "Doe", Email = "mail@gmail.com" };
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        userLogicMock.Setup(logic => logic.ExistUser(user.Id)).Returns(true);
        userLogicMock.Setup(logic => logic.DeleteUser(user.Id)).Returns(user);

        var userController = new AdminController(userLogicMock.Object);
        var result = userController.DeleteUser(user.Id);

        var expectedResponse = new OkObjectResult(user);
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void DeleteUser_WhenUserExists_ReturnsOkWithUserResponse()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);
        var userController = new UserController(userLogicMock.Object, homeLogicMock.Object);

        var userId = Guid.NewGuid();
        var expectedUser = new User
        {
            Id = userId,
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        };

        userLogicMock.Setup(logic => logic.DeleteUser(userId)).Returns(expectedUser);

        IActionResult result = userController.DeleteUser(userId);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var userResponse = okResult.Value as GetUserResponse;
        userResponse.Should().NotBeNull();
        userResponse.Name.Should().Be(expectedUser.Name);
        userResponse.LastName.Should().Be(expectedUser.LastName);
        userResponse.Email.Should().Be(expectedUser.Email);

        userLogicMock.Verify(logic => logic.DeleteUser(userId), Times.Once);
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
                Name = "Home",
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
                Name = "Home",
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
            Name = h.Name,
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
    public void GetUserHomes_ShouldReturnNoContent_WhenNoHomesFound()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();

        homeLogicMock.Setup(logic => logic.GetHomesByUser(userId))
            .Throws(new EmptyException("No homes found"));

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        Action act = () => controller.GetUserHomes(userId);

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No homes found")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        userLogicMock.VerifyAll();
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
                Name = "Home",
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
                Name = "Home",
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
            Name = h.Name,
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

    [TestMethod]
    public void GetUserNotifications_ShouldReturnNoContent_WhenNoNotificationsFound()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);

        var userId = Guid.NewGuid();

        userLogicMock.Setup(logic => logic.GetNotifications(userId))
            .Throws(new EmptyException("No notifications found"));

        var controller = new UserController(userLogicMock.Object, homeLogicMock.Object);

        Action act = () => controller.GetUserNotifications(userId);

        act.Should().Throw<EmptyException>();

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No notifications found")
        };

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        userLogicMock.VerifyAll();
    }

    [TestMethod]
    public void GetUserByMail_WhenUserExists_ReturnsOkWithUserResponse()
    {
        var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        var homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);
        var userController = new UserController(userLogicMock.Object, homeLogicMock.Object);

        var email = "mail@mail.com";

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = email
        };

        userLogicMock.Setup(logic => logic.FindByMail(email)).Returns(expectedUser);

        IActionResult result = userController.GetUserByEmail(email);

        var okResult = result as OkObjectResult;

        okResult.Should().NotBeNull();
        okResult.Value.Should().BeOfType<GetUserResponse>();
        okResult.Value.As<GetUserResponse>().Name.Should().Be(expectedUser.Name);
        okResult.Value.As<GetUserResponse>().LastName.Should().Be(expectedUser.LastName);
        okResult.Value.As<GetUserResponse>().Email.Should().Be(email);

        userLogicMock.VerifyAll();
    }

}
