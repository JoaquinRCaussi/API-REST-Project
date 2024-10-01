namespace Domain;

public sealed record class Role
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
}
