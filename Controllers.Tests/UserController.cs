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
        AdminRequest admin = new AdminRequest("John", "Doe", "JohnDoe@domain.com", "123456");
        Mock<IUserLogic> logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateAdmin(It.IsAny<User>())).Returns(admin.ToArgs());
        
        //Act 
        AdminController controller = new AdminController(logic.Object);
        IActionResult act =  controller.CreateAdmin(admin);
        
        AdminResponse adminResponse = new AdminResponse(admin.ToArgs());
        var expected = new OkObjectResult(adminResponse);

        // Assert
        act.Should().BeEquivalentTo(expected);
    }
    #endregion
}
