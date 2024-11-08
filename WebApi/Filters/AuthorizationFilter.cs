using System.Net;
using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    private readonly string? permission;

    public AuthorizationFilterAttribute(string? permission = null)
    {
        this.permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null)
        {
            return;
        }

        var userLogged = context.HttpContext.Items.ContainsKey(0) ? context.HttpContext.Items[0] : null;

        if (userLogged == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "UnAuthorized",
                Message = "Not authenticated"
            });
            return;
        }

        var userLoggedMapped = (User)userLogged;
        var permission = BuildPermission();
        var hasNotPermission = !UserHasPermission(userLoggedMapped, permission);
        if (context.RouteData.Values.ContainsKey("homeId"))
        {
            var homeRepository = (IHomeRepository)context.HttpContext.RequestServices.GetService(typeof(IHomeRepository));
            Home? home = null;

            if (homeRepository != null)
            {
                var homeId = Guid.Parse(context.RouteData.Values["homeId"].ToString() ?? throw new InvalidOperationException());
                home = homeRepository.GetHome(homeId);
            }
            if (home == null)
            {
                hasNotPermission = false;
            }
            else
            {
                hasNotPermission = !MemberHasPermission(home, userLoggedMapped, permission); ;
            }
        }

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

    private bool UserHasPermission(User? user, string? requiredPermission)
    {
        if (requiredPermission == null)
        {
            return true;
        }

        if (user.Role == null || user.Role.PermissionKeys == null)
        {
            return false;
        }

        return user.Role.PermissionKeys.Any(p => p.Value == requiredPermission);
    }

    public bool MemberHasPermission(Home home, User user, string? requiredPermission)
    {
        if (requiredPermission == null)
        {
            return true;
        }
        if (home == null || user == null || home.MemberSettings == null)
        {
            return false;
        }

        var memberSetting = home.MemberSettings.FirstOrDefault(ms => ms.UserId == user.Id);

        if (memberSetting == null || memberSetting.Permissions == null)
        {
            return false;
        }
        return memberSetting.Permissions.Any(p => p.Value == requiredPermission);
    }

    private string? BuildPermission()
    {
        return permission ?? null;
    }
}
