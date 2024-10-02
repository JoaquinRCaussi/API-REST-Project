using Domain;
using IDataAccess;
using LogicInterface;
using Models;

namespace BusinessLogic;

public class HomeLogic : IHomeLogic
{
    private readonly IHomeRepository _homeRepository;
    private readonly IMemberSettingRepository _memberSettingRepository;

    public HomeLogic(IHomeRepository homeRepository, IMemberSettingRepository memberSettingRepository)
    {
        _homeRepository = homeRepository;
        _memberSettingRepository = memberSettingRepository;
    }

    public Home CreateHome(Home home)
    {
        return _homeRepository.CreateHome(home);
    }

    public List<Home> GetHomes()
    {
        return _homeRepository.GetHomes();
    }

    public List<Home> GetHomesByUser(Guid userId)
    {
        return _homeRepository.GetHomesByUser(userId);
    }

    public Home GetHome(Guid homeId)
    {
        return _homeRepository.GetHome(homeId);
    }

    public List<User> GetHomeMembers(Guid homeId)
    {
        return _homeRepository.GetHomeMembers(homeId);
    }

    public Home AddMember(Guid homeId, Guid userId)
    {
        return _homeRepository.AddMember(homeId, userId);
    }
    
    public Home UpdatePermissions(Guid homeId, Guid userId, PermissionRequest permissions)
    {
        var permissionMappings = new Dictionary<Func<PermissionRequest, bool>, string>
        {
            { p => p.CanAddMembers, "CanAddMembers" },
            { p => p.CanAsociateDevices, "CanAsociateDevices" },
            { p => p.CanGetNotifications, "CanGetNotifications" },
            { p => p.CanListDevices, "CanListDevices" }
        };

        foreach (var mapping in permissionMappings)
        {
            var permissionName = mapping.Value;
            var hasPermission = mapping.Key(permissions);

            if (hasPermission)
            {
                // Si el permiso es true, agregarlo
                _memberSettingRepository.AddPermission(homeId, userId, permissionName);
            }
            else
            {
                // Si el permiso es false, verificar si ya existe y eliminarlo
                if (_memberSettingRepository.HasPermission(homeId, userId, permissionName))
                {
                    _memberSettingRepository.RemovePermission(homeId, userId, permissionName);
                }
            }
        }

        return _homeRepository.GetHome(homeId);
    }

}
