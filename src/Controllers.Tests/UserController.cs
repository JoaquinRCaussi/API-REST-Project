using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.DTOS;

namespace Controllers.Tests;

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
        var user = new User("John", "Doe", "JohnDoe@domain.com", "123456");
        Mock<IUserLogic> logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateAdmin(It.IsAny<User>())).Returns(user);
        
        //Act 
        var controller = new AdminController(logic.Object);
        var act =  controller.CreateAdmin(user);
        
        var UserResponse = new UserResponse(user);
        var expected = new OkObjectResult(UserResponse);

        // Assert
        act.Should().BeEquivalentTo(expected);
    }
    #endregion
}
