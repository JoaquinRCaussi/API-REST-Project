namespace Domain;

public sealed record class PermissionKey
{
    public Guid Id { get; set; }
    public string Value { get; init; }
    
    public Guid Role { get; set; }

    public PermissionKey() : this(string.Empty) { }

    public PermissionKey(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
