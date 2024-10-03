using System.Net;
using Azure;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, IActionResult> _errors = new Dictionary<Type, IActionResult>
    {
        {
            typeof(ConflictException),
            new ObjectResult(new { InnerCode = "Conflict", Message = "The resource already exists" })
            {
                StatusCode = (int)HttpStatusCode.Conflict
            } 
        },
        {
            typeof(NotValidDataException),
            new ObjectResult(new { InnerCode = "Bad Request", Message = "The request is not valid" })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            } 
        }
    };
    
    public void OnException(ExceptionContext? context)
    {
        var response = _errors.GetValueOrDefault(context.Exception.GetType());
        
        if (response == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError", Message = "There was an error when processing the request"
            }) { StatusCode = (int)HttpStatusCode.InternalServerError };
            return;
        }
        context.Result = response;
    }
}
