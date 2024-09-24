using System.Diagnostics.CodeAnalysis;
using Domain;
using DTOS;
using FluentAssertions;
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
        var act =  controller.CreateAdmin(admin);
        
        var adminResponse = new AdminResponse(admin.ToArgs());
        var expected = new OkObjectResult(adminResponse);

        // Assert
        act.Should().BeEquivalentTo(expected);
    }
    #endregion
}
