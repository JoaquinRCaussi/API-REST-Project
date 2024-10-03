using Domain;

namespace IBusinessLogic;

public interface IMemberSettingLogic
{
    MemberSetting CreateMemberSetting(Guid homeId, Guid userId);

    List<MemberSetting> GetMemberSettings();

    MemberSetting GetMemberSetting(Guid homeId, Guid userId);

    MemberSetting UpdateMemberSetting(MemberSetting memberSetting);

    MemberSetting DeleteMemberSetting(Guid memberSettingId);

    MemberSetting AddPermission(Guid homeId, Guid userId, string permission);

    MemberSetting RemovePermission(Guid homeId, Guid userId, string permission);

    bool HasPermission(Guid homeId, Guid userId, string permission);
}
