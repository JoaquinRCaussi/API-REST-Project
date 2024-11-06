namespace BusinessLogic.Entities;

public class Session
{
    public Guid UserID { get; set; }
    public Guid? RoleID { get; set; }
    public string? Token { get; set; }
    public User? User { get; set; }

}
