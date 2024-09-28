using IBusinessLogic;
using IDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class AuthenticationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"];
        
        if (String.IsNullOrEmpty(token))
        {
            context.Result = new JsonResult("Empty authorization header") { StatusCode = 401 };
        } 
        else if (!Guid.TryParse(token, out Guid parsedToken))
        {
            context.Result = new JsonResult("Invalid token format") { StatusCode = 400 };
        }
        else
        {
            var currentUser = GetSessionLogicService(context).GetCurrentUser(parsedToken);
            
            if (currentUser == null)
            {
                context.Result = new JsonResult("Inicie sesión") { StatusCode = 401 };
            }
        }
    }

    ISessionLogic GetSessionLogicService(AuthorizationFilterContext context)
    {
        var sessionManagerObject = context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
        var sessionService = sessionManagerObject as ISessionLogic;

        return sessionService;
    }
}
