using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class LoginControllerTest
{
    private Mock<ISessionService> _sessionLogicMock = null!;
    private LoginController _loginController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _sessionLogicMock = new Mock<ISessionService>();
        _loginController = new LoginController(_sessionLogicMock.Object);
    }

    [TestMethod]
    public void Login_ValidCredentials_ReturnsCreatedAtActionWithToken()
    {
        var userId = Guid.NewGuid();
        var userRoleId = Guid.NewGuid();

        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var authResult = new Session
        {
            Token = userId.ToString(),
            RoleID = userRoleId,
            UserID = userId
        };

        _sessionLogicMock.Setup(x => x.Authenticate(loginRequest.Email, loginRequest.Password)).Returns(authResult);

        var httpContext = new DefaultHttpContext();
        _loginController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var actionResult = _loginController.Login(loginRequest);

        // Assert
        actionResult.Should().BeOfType<CreatedAtActionResult>();
        var result = actionResult as CreatedAtActionResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(201);

        var response = result.Value as LoginResponse;
        response.Should().NotBeNull();
        response.Token.Should().Be(userId.ToString());

        _loginController.Response.Headers.Should().ContainKey("Authorization");
        _loginController.Response.Headers["Authorization"].ToString().Should().Be(userId.ToString());
    }





}
