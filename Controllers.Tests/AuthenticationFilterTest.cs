using System.Diagnostics.CodeAnalysis;
using System.Net;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Filters;

namespace Filters.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class AuthenticationFilterAttributeTest
{
    private Mock<ISessionService>? _sessionServiceMock;
    private AuthenticationFilterAttribute? _filter;

    [TestInitialize]
    public void Setup()
    {
        _sessionServiceMock = new Mock<ISessionService>();
        _filter = new AuthenticationFilterAttribute();
    }

    [TestMethod]
    public void OnAuthorization_WithoutAuthorizationHeader_ReturnsUnauthorized()
    {
        var context = CreateAuthorizationFilterContext(null);

        _filter.OnAuthorization(context);

        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "Unauthenticated",
            Message = "You are not authenticated"
        })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        });
    }

    [TestMethod]
    public void OnAuthorization_WithInvalidAuthorizationFormat_ReturnsUnauthorized()
    {
        var context = CreateAuthorizationFilterContext("InvalidFormat");

        _filter.OnAuthorization(context);

        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "InvalidAuthorization",
            Message = "The provided authorization header format is invalid"
        })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        });
    }

    [TestMethod]
    public void OnAuthorization_WithValidToken_SetsUserInHttpContext()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "John Doe" };
        var token = Guid.NewGuid();
        var stringToken = token.ToString();
        _sessionServiceMock.Setup(s => s.GetUserByToken(token)).Returns(user);

        var context = CreateAuthorizationFilterContext(stringToken);
        context.HttpContext.RequestServices = CreateServiceProvider().BuildServiceProvider();

        _filter.OnAuthorization(context);

        context.HttpContext.Items[0].Should().Be(user);
    }

    [TestMethod]
    public void OnAuthorization_WithBearerToken_SetsUserInHttpContext()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Jane Doe" };
        var token = Guid.NewGuid();
        var bearerToken = $"Bearer {token}";
        _sessionServiceMock.Setup(s => s.GetUserByToken(token)).Returns(user);

        var context = CreateAuthorizationFilterContext(bearerToken);
        context.HttpContext.RequestServices = CreateServiceProvider().BuildServiceProvider();

        _filter.OnAuthorization(context);

        context.HttpContext.Items[0].Should().Be(user);
    }

    [TestMethod]
    public void OnAuthorization_WithSessionServiceError_ThrowsException()
    {
        var token = Guid.NewGuid();
        var stringToken = token.ToString();
        _sessionServiceMock.Setup(s => s.GetUserByToken(token)).Throws(new Exception("Invalid Token"));

        var context = CreateAuthorizationFilterContext(stringToken);
        context.HttpContext.RequestServices = CreateServiceProvider().BuildServiceProvider();

        Action act = () => _filter.OnAuthorization(context);

        act.Should().Throw<Exception>().WithMessage("Invalid Token");
    }

    [TestMethod]
    public void OnAuthorization_MissingBearerPrefix_ParsesTokenCorrectly()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "John Without Bearer" };
        var token = Guid.NewGuid();
        _sessionServiceMock.Setup(s => s.GetUserByToken(token)).Returns(user);

        var context = CreateAuthorizationFilterContext(token.ToString());
        context.HttpContext.RequestServices = CreateServiceProvider().BuildServiceProvider();

        _filter.OnAuthorization(context);

        context.HttpContext.Items[0].Should().Be(user);
    }

    private AuthorizationFilterContext CreateAuthorizationFilterContext(string? authorizationHeader)
    {
        var httpContext = new DefaultHttpContext();
        if (authorizationHeader != null)
        {
            httpContext.Request.Headers["Authorization"] = authorizationHeader;
        }

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

    private IServiceCollection CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_sessionServiceMock.Object);
        return services;
    }
}
