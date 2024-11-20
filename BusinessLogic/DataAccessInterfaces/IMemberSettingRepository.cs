using BusinessLogic.Entities;

namespace BusinessLogic.DataAccessInterfaces;

public interface IMemberSettingRepository
{
    public MemberSetting CreateMemberSetting(Guid homeId, Guid userId);

    public List<MemberSetting> GetMemberSettings();

    public MemberSetting GetMemberSetting(Guid homeId, Guid userId);

    public MemberSetting UpdateMemberSetting(MemberSetting memberSetting);

    public MemberSetting DeleteMemberSetting(Guid memberSettingId);

    public MemberSetting AddPermission(Guid homeId, Guid userId, string permission);

    public MemberSetting RemovePermission(Guid homeId, Guid userId, string permission);

    public bool HasPermission(Guid homeId, Guid userId, string permission);
}
