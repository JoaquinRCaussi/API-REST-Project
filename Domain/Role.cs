namespace Domain;

public sealed record class Role
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public List<PermissionKey> Permissions { get; set; } = new List<PermissionKey>();
    
    public bool HasPermission(PermissionKey permission) => 
        Permissions.Contains(permission);
    
}
