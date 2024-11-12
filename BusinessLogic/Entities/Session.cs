namespace BusinessLogic.Entities;

public class Session
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public Guid UserID { get; set; }
    public Guid? RoleID { get; set; }
    public Guid? Token { get; set; }
    public User? User { get; set; }
    public DateTime? CreatedAt { get; set; }

}
