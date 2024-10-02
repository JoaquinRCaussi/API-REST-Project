namespace Domain;

public sealed record class PermissionKey
{
    public Guid Id { get; set; }
    public string? Value { get; init; }
    public List<Role> Roles { get; set; } = [];
}
