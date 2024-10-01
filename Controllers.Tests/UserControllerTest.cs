using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
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

        var controller = new UserController(userLogicMock.Object);

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

        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        userLogicMock.Setup(logic => logic.GetUser(It.IsAny<Guid>())).Returns(expectedUser);

        var controller = new UserController(userLogicMock.Object);

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
        userLogicMock.Setup(logic => logic.ExistUser(user.Id)).Returns(true);
        userLogicMock.Setup(logic => logic.DeleteUser(user.Id)).Returns(user);

        var userController = new UserController(userLogicMock.Object);
        var result = userController.DeleteUser(user.Id);

        var expectedResponse = new OkObjectResult(user);
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
