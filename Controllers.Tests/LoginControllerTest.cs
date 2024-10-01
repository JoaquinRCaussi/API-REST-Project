using Domain;
using IBusinessLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[TestClass]
public class LoginControllerTest
{
    private Mock<ISessionLogic> _sessionLogicMock = null!;
    private LoginController _loginController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _sessionLogicMock = new Mock<ISessionLogic>();
        _loginController = new LoginController(_sessionLogicMock.Object);
    }

    [TestMethod]
    public void Authenticate_ValidUser_ReturnsToken()
    {
        var email = "mail@mail.com";
        var password = "password@123";
        var token = Guid.NewGuid();
        
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };
        
        var authResult = new AuthenticationResult
        {
            UserId = token,
            RoleId = role.Id
        };
        
        _sessionLogicMock.Setup(x => x.Authenticate(email, password)).Returns(authResult);
        
        var result = _loginController.Login(loginRequest);
        
        result.Should().BeOfType<OkObjectResult>();
        
        var okResult = result as OkObjectResult;
        
        okResult?.Value.Should().BeOfType<LoginResponse>();
    }

}
