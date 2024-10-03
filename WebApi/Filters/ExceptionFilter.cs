using System.Net;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext? context)
    {
        context.Result = new ObjectResult(new
        {
            InnerCode = "InternalError", Message = "There was an error when processing the request"
        }) { StatusCode = (int)HttpStatusCode.InternalServerError };
    }
}
