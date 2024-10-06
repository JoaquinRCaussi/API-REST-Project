using System.Net;
using Domain;
using IDataAccess;
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

        var userLogged = context.HttpContext.Items[0];

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
        var hasNotType = false;

        if (context.RouteData.Values.ContainsKey("homeId"))
        {
            var homeRepository = (IHomeRepository)context.HttpContext.RequestServices.GetService(typeof(IHomeRepository));
            Home? home = null;

            if (homeRepository != null)
            {
                var homeId = Guid.Parse(context.RouteData.Values["homeId"].ToString() ?? throw new InvalidOperationException());
                home = homeRepository.GetHome(homeId);
            }

            hasNotPermission = home == null || !MemberHasPermission(home, userLoggedMapped, permission);

            //Checkeo para notificaciones de dispositivos
            if (context.RouteData.Values.ContainsKey("hardwareId"))
            {
                _ = homeRepository.GetHomeDevices(home.Id);
                var hardwareId = context.RouteData.Values["hardwareId"].ToString();
                var hardwareGuid = Guid.Empty;

                if (homeRepository != null)
                {
                    var homeId = Guid.Parse(context.RouteData.Values["homeId"].ToString() ?? throw new InvalidOperationException());
                    home = homeRepository.GetHome(homeId);
                }
                if (hardwareId != null)
                {
                    hardwareGuid = Guid.Parse(hardwareId);
                }

                var routeSegment = context.HttpContext.Request.Path.Value;
                if (routeSegment != null && home != null)
                {
                    if (routeSegment.Contains("/sensor/"))
                    {
                        hasNotType = !HardwareIdActuallyHasType(hardwareGuid, DeviceType.Sensor, home);
                    }
                    else if (routeSegment.Contains("/camera/"))
                    {
                        hasNotType = !HardwareIdActuallyHasType(hardwareGuid, DeviceType.Camera, home);
                    }
                }
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
        else if (hasNotType)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"HardwareId does not have the correct type"
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

    private bool MemberHasPermission(Home home, User user, string? requiredPermission)
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

    private bool HardwareIdActuallyHasType(Guid hardwareId, DeviceType type, Home home)
    {
        if (home == null || home.Devices == null)
        {
            return false;
        }

        var device = home.Devices.FirstOrDefault(d => d.HardwareId == hardwareId);

        if (device == null)
        {
            return false;
        }

        return device.Device.DeviceType == type;
    }

    private string? BuildPermission()
    {
        return permission ?? null;
    }
}
