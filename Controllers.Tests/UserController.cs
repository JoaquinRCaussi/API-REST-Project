using System.Diagnostics.CodeAnalysis;

using Domain;

using WebApi.Models;

using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class UserController
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
        var act = controller.CreateAdmin(admin);
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
            new User("John", "Doe", "df@domain.com", "123456"),
            new User("Jane", "Smith", "asd@domain.com", "12345asdas6")
        };
        
        userLogicMock.Setup(logic => logic.GetUsers()).Returns(expectedUsers);
        
        var controller = new UserController(userLogicMock.Object);
        
        var result = controller.GetUsers();
        
        var userResponses = expectedUsers.Select(u => new UserResponse(u)).ToList();
        var expectedResponse = new OkObjectResult(userResponses);
        
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
