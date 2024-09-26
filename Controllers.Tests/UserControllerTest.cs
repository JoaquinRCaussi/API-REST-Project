using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class UserControllerTest
{
    #region Admin

    [TestMethod]
    public void CreateAdmin_WhenAllPropertiesOk()
    {
        // Arrange
        var admin = new AdminRequest("John", "Doe", "JohnDoe@domain.com", "123456");
        var logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateAdmin(It.IsAny<User>())).Returns(admin.ToArgs());
        //Act 
        var controller = new AdminController(logic.Object);
        IActionResult act = controller.CreateAdmin(admin);
        var adminResponse = new AdminResponse(admin.ToArgs());
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
            new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "mail@mail.com",
                Password = "password@123"
            },
            new User
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

        var userResponses = expectedUsers.Select(u => new GetUserResponse(u)).ToList();
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
        
        var userResponse = new GetUserResponse(expectedUser);
        var expectedResponse = new OkObjectResult(userResponse);
        
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
