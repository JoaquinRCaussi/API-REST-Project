using System.Net;
using BusinessLogic;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute
    : Attribute,
    IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthenticated",
                Message = "You are not authenticated"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var isAuthorizationFormatNotValid = !IsAuthorizationFormatValid(authorizationHeader!);
        if (isAuthorizationFormatNotValid)
        {
            context.Result = new ObjectResult(
                new
                {
                    InnerCode = "InvalidAuthorization",
                    Message = "The provided authorization header format is invalid"
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var isAuthorizationExpired = IsAuthorizationExpired();
        if (isAuthorizationExpired)
        {
            context.Result = new ObjectResult(
                new
                {
                    InnerCode = "ExpiredAuthorization",
                    Message = "The provided authorization header is expired"
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var token = authorizationHeader!.Split(" ")[1];

        try
        {
            var userOfAuthorization = GetUserOfAuthorization(token, context);

            context.HttpContext.Items[0] = userOfAuthorization;
        }
        catch (Exception)
        {
            throw new NotValidDataException("Invalid token");
        }
    }

    private bool IsAuthorizationFormatValid(string authorization)
    {
        return authorization.StartsWith("Bearer ");
    }

    private bool IsAuthorizationExpired()
    {
        return false;
    }

    private User GetUserOfAuthorization(
        string authorization,
        AuthorizationFilterContext context)
    {
        var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();

        var user = sessionService.GetUserByToken(authorization);

        return user;
    }
}

