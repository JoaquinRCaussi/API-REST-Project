using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class AddMemberResponse
{
    public Home? Home { get; set; }
    public MemberSetting? MemberSetting { get; set; }
}

