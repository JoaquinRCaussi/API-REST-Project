using System.Diagnostics.CodeAnalysis;
using Domain;
using DTOS;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;


namespace Controllers.Tests;
[ExcludeFromCodeCoverage]
[TestClass]
public class UserController
{
    

    [TestInitialize]
    public void Initialize()
    {
        
    }

    #region Admin
    [TestMethod]
    public void CreateAdmin_WhenAllPropertiesOk()
    {
        // Arrange
        UserRequest user = new UserRequest("John", "Doe", "JohnDoe@domain.com", "123456");
        Mock<IUserLogic> logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateAdmin(It.IsAny<User>())).Returns(user.ToArgs());
        
        //Act 
        AdminController controller = new AdminController(logic.Object);
        IActionResult act =  controller.CreateAdmin(user);
        
        UserResponse UserResponse = new UserResponse(user.ToArgs());
        OkObjectResult expected = new OkObjectResult(UserResponse);

        // Assert
        act.Should().BeEquivalentTo(expected);
    }
    #endregion
}
