using System.Net;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, HttpStatusCode> _errorStatusCodes = new Dictionary<Type, HttpStatusCode>
    {
        { typeof(ConflictException), HttpStatusCode.Conflict },
        { typeof(NotValidDataException), HttpStatusCode.BadRequest }
    };
    public void OnException(ExceptionContext? context)
    {
        var exceptionType = context.Exception.GetType();

        if (_errorStatusCodes.TryGetValue(exceptionType, out var statusCode))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = statusCode.ToString(),
                Message = context.Exception.Message
            })
            {
                StatusCode = (int)statusCode
            };
        }
        else
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError",
                Message = context.Exception.Message
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}

