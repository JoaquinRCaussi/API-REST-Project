using System.Net;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilterAttribute(string? permission = null)
    : Attribute,
        IAuthorizationFilter
{

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null)
        {
            return;
        }

        var userLogged = context.HttpContext.Items[0];

        var userIsNotIdentified = userLogged == null;
        if (userIsNotIdentified)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "UnAuthorized",
                Message = $"Not authenticated"
            });
            return;
        }
        var userLoggedMapped = (User)userLogged;

        var permission = BuildPermission(context);

        var hasNotPermission = !UserHasPermission(userLoggedMapped, permission);

        if (hasNotPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"Missing permission {permission}"
            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }
    }

    private bool UserHasPermission(User? user, string requiredPermission)
    {
        if (user.Role == null || user.Role.PermissionKeys == null)
        {
            return false;
        }

        return user.Role.PermissionKeys.Any(p => p.Value == requiredPermission);
    }

    private string BuildPermission(AuthorizationFilterContext context)
    {
        return permission ?? $"{context.RouteData.Values["action"].ToString().ToLower()}-{context.RouteData.Values["controller"].ToString().ToLower()}";
    }
}
