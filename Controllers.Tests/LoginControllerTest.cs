using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;
using WebApi.Models.Out;

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
            Token = userId,
            RoleID = userRoleId,
            UserID = userId
        };

        _sessionLogicMock.Setup(x => x.Authenticate(loginRequest.Email, loginRequest.Password)).Returns(authResult);

        var httpContext = new DefaultHttpContext();
        _loginController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var actionResult = _loginController.Login(loginRequest);

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
