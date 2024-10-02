namespace Domain;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public Guid? RoleID { get; set; } = null;
    public Role Role { get; set; } = null;
    public Guid? CompanyID { get; set; } = null;
    public Company Company { get; set; } = null;
}
