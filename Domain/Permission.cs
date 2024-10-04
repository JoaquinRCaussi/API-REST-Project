using System.Text.Json.Serialization;

namespace Domain;

public class Permission
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Value { get; set; } = string.Empty;

    [JsonIgnore]
    public virtual List<MemberSetting> MemberSettings { get; set; } = [];
}
