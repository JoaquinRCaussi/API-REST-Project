using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using WebApi.Filters;

namespace Filters.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class ExceptionFilterTest
{
    private ExceptionContext? _context;
    private ExceptionFilter _attribute;

    public ExceptionFilterTest()
    {
        _attribute = new ExceptionFilter();
    }
    
    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }
    
    [TestMethod]
    public void OnException_WhenExceptionIsThrown_ShouldReturnInternalServerError()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _context.Exception = exception;
        
        // Act
        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var objectResult = response as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be((int)StatusCodes.Status500InternalServerError);
        GetInnerCode(objectResult.Value).Should().Be("InternalError");
        GetInnerMessage(objectResult.Value).Should().Be(exception.Message);
    }

    private string GetInnerCode(object? value)
    {
        return value.GetType().GetProperty("InnerCode")?.GetValue(value)?.ToString() ?? string.Empty;
    }
    
    private string GetInnerMessage(object? value)
    {
        return value.GetType().GetProperty("Message")?.GetValue(value)?.ToString() ?? string.Empty;
    }
    
}
