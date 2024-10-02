using DataAccess.Data;
using Domain;
using IDataAccess;

namespace DataAccess.Repositories;

public class MemberSettingRepository : IMemberSettingRepository
{
    private readonly HMDbContext _dbContext;

    public MemberSettingRepository(HMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public MemberSetting CreateMemberSetting(Guid homeId, Guid userId)
    {
        var user = _dbContext.Users?.FirstOrDefault(x => x.Id == userId);
        var home = _dbContext.Homes?.FirstOrDefault(x => x.Id == homeId);
        var permission = _dbContext.Permissions?.FirstOrDefault(x => x.Value == "CanGetNotifications");

        if (permission != null)
        {
            var memberSetting = new MemberSetting
            {
                HomeId = homeId,
                UserId = userId,
            };
            
            _dbContext.MemberSettings?.Add(memberSetting);
            memberSetting.Permissions.Add(permission);
            _dbContext.SaveChanges();
            return memberSetting;
        }
        else
        {
            var memberSetting = new MemberSetting
            {
                HomeId = homeId,
                UserId = userId,
            };
            
            _dbContext.MemberSettings?.Add(memberSetting);
            _dbContext.SaveChanges();
            return memberSetting;
        }
        
    }

    public List<MemberSetting> GetMemberSettings()
    {
        return _dbContext.MemberSettings?.ToList()!;
    }
    
    public MemberSetting GetMemberSetting(Guid homeId, Guid userId)
    {
        return _dbContext.MemberSettings?.FirstOrDefault(x => x.HomeId == homeId && x.UserId == userId) ?? throw new InvalidOperationException();
    }

    public MemberSetting UpdateMemberSetting(MemberSetting memberSetting)
    {
        _dbContext.MemberSettings?.Update(memberSetting);
        _dbContext.SaveChanges();
        return memberSetting;
    }

    public MemberSetting DeleteMemberSetting(Guid memberSettingId)
    {
        var memberSetting = _dbContext.MemberSettings?.FirstOrDefault(x => x.Id == memberSettingId);
        if (memberSetting == null)
        {
            return null;
        }

        _dbContext.MemberSettings?.Remove(memberSetting);
        _dbContext.SaveChanges();
        return memberSetting;
    }
    
    public MemberSetting AddPermission(Guid homeId, Guid userId, string permission)
    {
        var memberSetting = _dbContext.MemberSettings?.FirstOrDefault(x => x.HomeId == homeId && x.UserId == userId);
        if (memberSetting == null)
        {
            return null;
        }
        
        var permit = _dbContext.Permissions?.FirstOrDefault(x => x.Value == permission);
        
        if(permit == null)
        {
            return null;
        }
        
        memberSetting.Permissions.Add(permit);
        _dbContext.SaveChanges();
        return memberSetting;
    }
    
    public MemberSetting RemovePermission(Guid homeId, Guid userId, string permission)
    {
        var memberSetting = _dbContext.MemberSettings?.FirstOrDefault(x => x.HomeId == homeId && x.UserId == userId);
        if (memberSetting == null)
        {
            return null;
        }
        
        var permit = _dbContext.Permissions?.FirstOrDefault(x => x.Value == permission);
        
        if(permit == null)
        {
            return null;
        }
        
        memberSetting.Permissions.Remove(permit);
        _dbContext.SaveChanges();
        return memberSetting;
    }
    
    public bool HasPermission(Guid homeId, Guid userId, string permission)
    {
        var memberSetting = _dbContext.MemberSettings?.FirstOrDefault(x => x.HomeId == homeId && x.UserId == userId);
        if (memberSetting == null)
        {
            return false;
        }
        
        var permit = _dbContext.Permissions?.FirstOrDefault(x => x.Value == permission);
        
        if(permit == null)
        {
            return false;
        }
        
        return memberSetting.Permissions.Contains(permit);
    }
}
