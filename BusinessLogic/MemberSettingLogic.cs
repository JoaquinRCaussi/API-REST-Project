using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class MemberSettingLogic : IMemberSettingLogic
{
    private readonly IMemberSettingRepository _memberSettingRepository;
    
    public MemberSettingLogic(IMemberSettingRepository memberSettingRepository)
    {
        _memberSettingRepository = memberSettingRepository;
    }
    
    public MemberSetting CreateMemberSetting(Guid homeId, Guid userId)
    {
        return _memberSettingRepository.CreateMemberSetting(homeId, userId);
    }
    
    public List<MemberSetting> GetMemberSettings()
    {
        return _memberSettingRepository.GetMemberSettings();
    }
    
    public MemberSetting GetMemberSetting(Guid homeId, Guid userId)
    {
        return _memberSettingRepository.GetMemberSetting(homeId, userId);
    }
    
    public MemberSetting UpdateMemberSetting(MemberSetting memberSetting)
    {
        return _memberSettingRepository.UpdateMemberSetting(memberSetting);
    }
    
    public MemberSetting DeleteMemberSetting(Guid memberSettingId)
    {
        return _memberSettingRepository.DeleteMemberSetting(memberSettingId);
    }
    
    public MemberSetting AddPermission(Guid homeId, Guid userId, string permission)
    {
        return _memberSettingRepository.AddPermission(homeId, userId, permission);
    }
    
    public MemberSetting RemovePermission(Guid homeId, Guid userId, string permission)
    {
        return _memberSettingRepository.RemovePermission(homeId, userId, permission);
    }
    
    public bool HasPermission(Guid homeId, Guid userId, string permission)
    {
        return _memberSettingRepository.HasPermission(homeId, userId, permission);
    }
}
