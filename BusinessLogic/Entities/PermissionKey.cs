using System.Text.Json.Serialization;

namespace BusinessLogic.Entities;

public sealed record class PermissionKey
{
    public Guid Id { get; set; }
    public string? Value { get; init; }

    [JsonIgnore]
    public List<Role>? Roles { get; set; } = [];
}
