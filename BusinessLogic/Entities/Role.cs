namespace BusinessLogic.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;

    public virtual List<PermissionKey> PermissionKeys { get; set; } = [];
}
