using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Domain;
using LogicInterface;
using WebApi.Models;
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
            new("John", "Doe", "df@domain.com", "123456"), new("Jane", "Smith", "asd@domain.com", "12345asdas6")
        };

        userLogicMock.Setup(logic => logic.GetUsers()).Returns(expectedUsers);

        var controller = new UserController(userLogicMock.Object);

        IActionResult result = controller.GetUsers();

        var userResponses = expectedUsers.Select(u => new GetUserResponse(u)).ToList();
        var expectedResponse = new OkObjectResult(userResponses);

        result.Should().BeEquivalentTo(expectedResponse);
    }
}
