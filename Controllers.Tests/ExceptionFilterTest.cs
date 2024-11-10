using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
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
    private readonly ExceptionFilter _attribute;

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

    private string GetInnerCode(object? value)
    {
        return value.GetType().GetProperty("InnerCode")?.GetValue(value)?.ToString() ?? string.Empty;
    }

    private string GetInnerMessage(object? value)
    {
        return value.GetType().GetProperty("Message")?.GetValue(value)?.ToString() ?? string.Empty;
    }

    [TestMethod]
    public void OnException_WhenExceptionIsThrown_ShouldReturnInternalServerError()
    {
        var exception = new Exception("Test exception");
        _context.Exception = exception;

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var objectResult = response as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be((int)StatusCodes.Status500InternalServerError);
        GetInnerCode(objectResult.Value).Should().Be("InternalError");
        GetInnerMessage(objectResult.Value).Should().Be(exception.Message);
    }

    [TestMethod]
    public void OnConflictException_WhenExceptionIsThrown_ShouldReturnConflict()
    {
        var exception = new ConflictException("Test exception");
        _context.Exception = exception;

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var objectResult = response as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be((int)StatusCodes.Status409Conflict);
        GetInnerCode(objectResult.Value).Should().Be("Conflict");
        GetInnerMessage(objectResult.Value).Should().Be(exception.Message);
    }

    [TestMethod]
    public void OnNotValidDataException_WhenExceptionIsThrown_ShouldReturnBadRequest()
    {
        var exception = new NotValidDataException("Test exception");
        _context.Exception = exception;

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var objectResult = response as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be((int)StatusCodes.Status400BadRequest);
        GetInnerCode(objectResult.Value).Should().Be("BadRequest");
        GetInnerMessage(objectResult.Value).Should().Be(exception.Message);
    }

    [TestMethod]
    public void OnEmptyException_WhenExceptionIsThrown_ShouldReturnNoContent()
    {
        var exception = new EmptyException("Test exception");
        _context.Exception = exception;

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var objectResult = response as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be((int)StatusCodes.Status204NoContent);
        GetInnerCode(objectResult.Value).Should().Be("NoContent");
        GetInnerMessage(objectResult.Value).Should().Be(exception.Message);
    }

}
