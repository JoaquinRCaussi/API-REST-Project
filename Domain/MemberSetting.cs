namespace Domain;

public class MemberSetting
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid HomeId { get; set; }
    public virtual List<Permission> Permissions { get; set; } = [];
}
